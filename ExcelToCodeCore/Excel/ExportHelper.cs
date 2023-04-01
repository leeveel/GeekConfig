using DotLiquid;
using ExcelConverter.Utils;
using ExcelToCodeCore.Utils;
using MessagePack;
using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace ExcelToCode.Excel
{
    public class ExportHelper
    {

        private static readonly NLog.Logger LOGGER = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 是否初始化 
        /// </summary>
        private static bool IsInited = false;

        public static void Init()
        {
            if (IsInited)
                return;
            IsInited = true;
        }

        static void ParseSelfDefType(string filePath)
        {
            var tableDatas = ExcelReader.ReadAllData(filePath);
            foreach (var sheetDatas in tableDatas)
            {
                if (sheetDatas.Key == "enumdef")
                {
                    foreach (var rowData in sheetDatas.Value)
                    {
                        if (rowData.Count == 0 || string.IsNullOrEmpty(rowData[0]))
                        {
                            continue;
                        }
                        var newEnum = new EnumType();
                        newEnum.Name = rowData[0];
                        for (int i = 1; i < rowData.Count; i++)
                        {
                            if (string.IsNullOrEmpty(rowData[i]))
                                continue;
                            newEnum.AddField(rowData[i].Trim());
                        }
                        DataType.AddEnum(newEnum);
                    }
                }
                else if (sheetDatas.Key == "classdef")
                {
                    foreach (var rowData in sheetDatas.Value)
                    {
                        if (rowData.Count == 0 || string.IsNullOrEmpty(rowData[0]))
                        {
                            continue;
                        }
                        var newClass = new ClassType();
                        newClass.Name = rowData[0];
                        for (int i = 1; i < rowData.Count; i++)
                        {
                            if (string.IsNullOrEmpty(rowData[i]))
                                continue;
                            var strs = rowData[i].Trim().Split(":", StringSplitOptions.RemoveEmptyEntries);
                            if (strs.Length < 2)
                            {
                                LogUtil.AddIgnoreLog(filePath, "classdef", $"错误的类字段定义{rowData[i]}");
                                continue;
                            }

                            var err = newClass.AddField(strs[0].Trim(), strs[1].Trim());
                            if (err != null)
                            {
                                LogUtil.AddIgnoreLog(filePath, "classdef", err);
                            }
                        }
                        DataType.AddClass(newClass);
                    }
                }
            }
        }

        public static async Task<bool> Export(ExportType etype, List<string> fileList, List<string> coverFileList, bool isAll)
        {
            Init();
            DataType.Init();

            fileList = new List<string>(fileList.ToArray());

            //只有全部导出的时候才清空，导出单个文件不清空
            if (isAll)
            {
                //清空原来的代码目录
                string path = Setting.GetCodePath(etype);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                else
                    FileUtil.ClearDirectory(path);

                //清空原来的Bin目录
                path = Setting.GetBinPath(etype);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                else
                    FileUtil.ClearDirectory(path);
            }

            LogUtil.Add("覆盖配置数量： " + (coverFileList != null ? coverFileList.Count : 0));
            var coverMap = new Dictionary<string, string>();
            if (coverFileList != null)
            {
                foreach (var file in coverFileList)
                    coverMap[Path.GetFileName(file)] = file;
            }

            //判断是否有自定义类型表，有的话，先生成类型信息
            var selfDefFile = fileList.Find(f => f.Contains("typedefine.xlsx"));
            if (selfDefFile != null)
            {
                ParseSelfDefType(selfDefFile);
                fileList.Remove(selfDefFile);
            }

            DataMgrInfo mgrInfo = new DataMgrInfo();
            mgrInfo.Enumtypes = DataType.selfEnumMapper.Values.ToList();
            mgrInfo.Classtypes = DataType.selfClassMapper.Values.ToList();
            var coverErr = "";
            for (int i = 0; i < fileList.Count; i++)
            {
                ExcelReader excelReader = new ExcelReader();
                ExcelPackage package = null;
                List<SheetHeadInfo> headInfos = excelReader.ReadHeadInfo(fileList[i], etype, out package);

                //覆盖数据
                ExcelPackage coverPackage = null;
                var fileName = Path.GetFileName(fileList[i]);
                if (coverMap.ContainsKey(fileName))
                {
                    var coverHeadInfos = new ExcelReader().ReadHeadInfo(coverMap[fileName], etype, out coverPackage);
                    //检测2张表的字段个数和顺序是否一致
                    for (int x = 0; x < headInfos.Count || x < coverHeadInfos.Count; ++x)
                    {
                        if (coverHeadInfos.Count <= x)
                            coverErr = "差异表少了一个sheet:" + headInfos[x].SheetName;
                        if (headInfos.Count <= x)
                            coverErr = "差异表多了一个sheet:" + coverHeadInfos[x].SheetName;
                        if (headInfos[x].SheetName != coverHeadInfos[x].SheetName)
                            coverErr = $"原表和差异表第【{x}】个sheet 名字不相同【{headInfos[x].SheetName}】 【{coverHeadInfos[x].SheetName}】";
                        for (int y = 0; y < headInfos[x].FieldCount || y < coverHeadInfos[x].FieldCount; ++y)
                        {
                            if (coverHeadInfos[x].FieldCount <= y)
                                coverErr = $"差异表【{headInfos[x].SheetName}】少了一个字段:【{headInfos[x].Fields[y].Name}】";
                            if (headInfos[x].FieldCount <= y)
                                coverErr = $"差异表【{coverHeadInfos[x].SheetName}】多了一个字段:【{coverHeadInfos[x].Fields[y].Name}】";
                            if (headInfos[x].Fields[y].Name != coverHeadInfos[x].Fields[y].Name)
                                coverErr = $"原表和差异表【{headInfos[x].SheetName}】sheet第【{y}】个字段名字不相同【{headInfos[x].Fields[y].Name}】【{coverHeadInfos[x].Fields[y].Name}】";
                            if (headInfos[x].Fields[y].Datatype != coverHeadInfos[x].Fields[y].Datatype)
                                coverErr = $"原表和差异表【{headInfos[x].SheetName}】sheet第【{y}】个字段【{headInfos[x].Fields[y].Name}】 类型不相同【{headInfos[x].Fields[y].Datatype}】【{coverHeadInfos[x].Fields[y].Datatype}】";
                            if (!string.IsNullOrEmpty(coverErr))
                            {
                                LogUtil.Add(coverErr, true);
                                return false;
                            }
                        }
                        if (!string.IsNullOrEmpty(coverErr))
                        {
                            LogUtil.Add(coverErr, true);
                            return false;
                        }
                    }
                }

                GenBin(headInfos, package, coverPackage, etype);
                GenBeanAddContainer(headInfos, etype, mgrInfo);

                LogUtil.AddNormalLog(package.File.Name, "导表完成");

                if (package != null)
                    package.Dispose();
            }

            GenSelfDef(etype, mgrInfo);
            GenGameDataManager(mgrInfo, etype, isAll);
            await GenMessagePackFormatters(mgrInfo, etype, isAll);
            return true;
        }

        private static async Task GenMessagePackFormatters(DataMgrInfo mgrInfo, ExportType etype, bool isAll)
        {
            if (!isAll || etype == ExportType.Unknown || etype == ExportType.Server)
            {
                return;
            }
            string input = Setting.GetCodePath(etype) + @"/Data/";
            string output = Setting.GetCodePath(etype) + @"/Data/Formatter";
            await MessagePackFormattersGen.RunAsync(input, output);
        }

        private static void GenSelfDef(ExportType etype, DataMgrInfo mgrInfo)
        {
            string targetPath = Setting.GetCodePath(etype) + @"/Data/Define/";
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            string templatePath = Setting.GetTemplatePath(etype) + "/EnumDef.template";
            Template template = Template.Parse(File.ReadAllText(templatePath));
            var content = template.Render(Hash.FromAnonymousObject(mgrInfo));
            File.WriteAllText(targetPath + "/DataEnum.cs", content);

            templatePath = Setting.GetTemplatePath(etype) + "/ClassDef.template";
            template = Template.Parse(File.ReadAllText(templatePath));
            content = template.Render(Hash.FromAnonymousObject(mgrInfo));
            File.WriteAllText(targetPath + "/DataClass.cs", content);
        }

        private static void GenBeanAddContainer(List<SheetHeadInfo> headInfos, ExportType etype, DataMgrInfo mgrInfo)
        {
            string beanPath = Setting.GetCodePath(etype) + @"/Data/Beans/";
            if (!Directory.Exists(beanPath))
                Directory.CreateDirectory(beanPath);
            string containerPath = Setting.GetCodePath(etype) + @"/Data/Containers/";
            if (!Directory.Exists(containerPath))
                Directory.CreateDirectory(containerPath);


            string content = "";
            string templatePath = Setting.GetTemplatePath(etype) + "/Bean.template";
            Template template = Template.Parse(File.ReadAllText(templatePath));
            foreach (var info in headInfos)
            {
                if (info.SheetName == "t_language")
                {
                    string lanTemplatePath = Setting.GetTemplatePath(etype) + "/LanguageBean.template";
                    Template lanTemplate = Template.Parse(File.ReadAllText(lanTemplatePath));
                    content = lanTemplate.Render(Hash.FromAnonymousObject(info));
                    File.WriteAllText(beanPath + info.BeanClassName + ".cs", content);
                }
                else
                {
                    content = template.Render(Hash.FromAnonymousObject(info));
                    File.WriteAllText(beanPath + info.BeanClassName + ".cs", content);
                }
            }

            templatePath = Setting.GetTemplatePath(etype) + "/Container.template";
            var commonTemplate = Template.Parse(File.ReadAllText(templatePath));

            templatePath = Setting.GetTemplatePath(etype) + "/LanguageContainer.template";
            var langTemplate = Template.Parse(File.ReadAllText(templatePath));

            foreach (var info in headInfos)
            {
                content = (info.SheetName == "t_language" ? langTemplate : commonTemplate).Render(Hash.FromAnonymousObject(info));
                File.WriteAllText(containerPath + info.ContainerClassName + ".cs", content);
            }

            for (int i = 0; i < headInfos.Count; i++)
            {
                mgrInfo.Containers.Add(headInfos[i].ContainerClassName);
            }
        }

        private static void GenGameDataManager(DataMgrInfo mgrInfo, ExportType etype, bool isAll)
        {
            string path = Setting.GetCodePath(etype) + @"/GameDataManager.cs";
            if (isAll || !File.Exists(path))
            {
                string templatePath = Setting.GetTemplatePath(etype) + "/GameDataManager.template";
                Template template = Template.Parse(File.ReadAllText(templatePath));
                var str = template.Render(Hash.FromAnonymousObject(mgrInfo));
                File.WriteAllText(Setting.GetCodePath(etype) + "GameDataManager.cs", str);
            }
            else
            {
                if (mgrInfo == null || mgrInfo.Containers.Count <= 0)
                    return;
                string containerName = mgrInfo.Containers[0];

                string part1 = string.Format("{0} {1} = new {2}();", containerName, containerName, containerName);
                string part2 = string.Format("t_containerMap.Add({0}.BinType, {1});", containerName, containerName);
                string part3 = string.Format("LoadOneBean({0}.BinType, forceReload);", containerName);

                string content = File.ReadAllText(path);
                int index = content.IndexOf("@%@%@");
                if (index != -1)
                {
                    if (content.IndexOf(part1) < 0)
                        content = content.Insert(index - 6, "\r\n\t\t" + part1);
                }
                else
                {
                    LogUtil.Add("找不到标记位：@%@%@", true);
                }

                index = content.IndexOf("@#@#@");
                if (index != -1)
                {
                    if (content.IndexOf(part2) < 0)
                        content = content.Insert(index - 6, "\r\n\t\t\t" + part2);
                }
                else
                {
                    LogUtil.Add("找不到标记位：@#@#@", true);
                }

                index = content.IndexOf("@*@*@");
                if (index != -1)
                {
                    if (content.IndexOf(part3) < 0)
                        content = content.Insert(index - 6, "\r\n\t\t\t" + part3);
                }
                else
                {
                    LogUtil.Add("找不到标记位：@*@*@", true);
                }

                File.WriteAllText(Setting.GetCodePath(etype) + "GameDataManager.cs", content);
            }
        }


        private static object GetTrueValue(string tableFile, string sheetName, string content, string elementType, bool isArray, string splitChar)
        {
            if (isArray)
            {
                var list = new List<object>();
                var strs = content.Split(splitChar);
                foreach (var s in strs)
                {
                    if (string.IsNullOrWhiteSpace(content))
                        continue;
                    list.Add(GetTrueValue(tableFile, sheetName, s, elementType, false, splitChar));
                }
                return list;
            }
            else
            {
                object value = null;
                switch (elementType)
                {
                    case "int":
                    case DataType.TextMult:
                        int intVal = 0;
                        int.TryParse(content, out intVal);
                        value = intVal;
                        break;
                    case "string":
                        if (string.IsNullOrEmpty(content))
                            content = "";
                        content = content.Trim();
                        //处理换行符
                        content = content.Replace(@"\n", "\n");
                        value = content;
                        break;
                    case "float":
                        float floatVal = 0;
                        float.TryParse(content, out floatVal);
                        value = floatVal;
                        break;
                    case "long":
                        long longVal = 0;
                        long.TryParse(content, out longVal);
                        value = longVal;
                        break;
                    default:
                        if (string.IsNullOrEmpty(elementType))
                        {
                            LogUtil.AddIgnoreLog(tableFile, sheetName, $"未知的类型 {content},可能没有选择@typedefine表");
                            break;
                        }
                        if (DataType.IsEnum(elementType))
                        {
                            var v = DataType.GetEnumValue(elementType, content.Trim());
                            value = v;
                            if (v == -1)
                            {
                                LogUtil.AddIgnoreLog(tableFile, sheetName, $"没有定义的枚举字段{elementType} {content}");
                            }
                            break;
                        }
                        if (DataType.IsClass(elementType))
                        {
                            var classDef = DataType.GetClassType(elementType);
                            var valueDic = new Dictionary<string, object>();
                            var strs = content.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            foreach (var str in strs)
                            {
                                if (str.Contains(":"))
                                {
                                    var strs2 = str.Split(":", StringSplitOptions.RemoveEmptyEntries);
                                    if (strs2.Length < 2)
                                    {
                                        LogUtil.AddIgnoreLog(tableFile, sheetName, $"解析自定义class失败 {elementType} {content},格式错误，不是filed:value格式");
                                    }
                                    else
                                    {
                                        if (classDef.FieldsMap.TryGetValue(strs2[0], out var field))
                                        {
                                            var key = strs2[0];
                                            if (field.Elementtype == "textmult" && !field.IsArray)
                                            {
                                                key = "m_" + key;
                                            }
                                            valueDic[key] = GetTrueValue(tableFile, sheetName, strs2[1], field.Elementtype, field.IsArray, field.ArraySplitChar);
                                        }
                                        else
                                        {
                                            LogUtil.AddIgnoreLog(tableFile, sheetName, $"解析自定义class失败：{elementType} {content}不能发现字段{strs2[0]}");
                                        }
                                    }
                                }
                                else
                                {
                                    LogUtil.AddIgnoreLog(tableFile, sheetName, $"解析自定义class失败 {elementType} {content}");
                                }
                            }
                            value = valueDic;
                            break;
                        }
                        break;
                }
                return value;
            }
        }


        private static void GenBin(List<SheetHeadInfo> headInfos, ExcelPackage package, ExcelPackage coverPackage, ExportType etype)
        {
            //var msgPackOption = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

            for (int i = 0; i < headInfos.Count; i++)
            {
                SheetHeadInfo headInfo = headInfos[i];
                ExcelWorksheet sheet = package.Workbook.Worksheets[headInfo.SheetId]; //只导出合法表单id的数据 
                                                                                      //空表没有数据
                if (ExcelReader.DataStartRow > sheet.Dimension.End.Row)
                    continue;

                var proxy = new SheetSerializeProxy<object>();
                proxy.sheetName = headInfos[i].SheetName;
                var fieldNames = new List<string>();

                bool splitColumn = headInfo.SheetName == "t_language";

                for (int k = 0; k < headInfo.Fields.Count; k++)
                {
                    var content = "";
                    Field field = headInfo.Fields[k];
                    content = field.Name;
                    if (string.IsNullOrEmpty(content))
                        content = "";
                    content = content.Trim();
                    //处理换行符
                    content = content.Replace(@"\n", "\n");
                    fieldNames.Add(content);
                }

                //读取cover数据
                ExcelWorksheet coverSheet = null;
                if (coverPackage != null)
                {
                    foreach (var tempSheet in coverPackage.Workbook.Worksheets)
                    {
                        if (tempSheet.Name == sheet.Name)
                        {
                            coverSheet = tempSheet;
                            break;
                        }
                    }
                }
                var coverDataMap = new Dictionary<string, int>();//key-line
                if (coverSheet != null)
                {
                    for (int m = ExcelReader.DataStartRow, n = coverSheet.Dimension.End.Row; m <= n; m++)
                    {
                        var content = "";
                        int col = headInfos[i].Fields[0].Col;
                        var obj = coverSheet.GetValue(m, col);
                        if (obj != null)
                            content = obj.ToString();
                        if (string.IsNullOrEmpty(content))
                            continue;
                        coverDataMap[content] = m;
                    }
                    LogUtil.Add($"{sheet.Name} 覆盖数据：{coverDataMap.Count}条");
                }

                //按列保存数据
                var emptyLine = new List<int>();
                var rowCount = sheet.Dimension.End.Row;
                for (int m = 0; m < fieldNames.Count; m++)
                {
                    var data = new List<object>();
                    proxy.datas[fieldNames[m]] = data;

                    Field field = headInfos[i].Fields[m];

                    for (int n = ExcelReader.DataStartRow; n <= rowCount; n++)
                    {
                        var content = "";
                        var obj = sheet.GetValue(n, m + 1);
                        if (obj != null)
                            content = obj.ToString();

                        //覆盖数据
                        if (m != 0)
                        {
                            var idStr = sheet.GetValue(n, 1)?.ToString();
                            if (idStr != null && coverDataMap.ContainsKey(idStr))
                            {
                                content = "";
                                obj = coverSheet.GetValue(coverDataMap[idStr], m + 1);
                                if (obj != null)
                                    content = obj.ToString();
                            }
                        }

                        if (content == "" && field.Name == "t_id")
                        {
                            emptyLine.Add(n - ExcelReader.DataStartRow);
                        }


                        data.Add(GetTrueValue(package.File.Name, sheet.Name, content, field.Elementtype, field.IsArray, field.ArraySplitChar));
                    }
                }

                for (int l = emptyLine.Count - 1; l >= 0; l--)
                {
                    foreach (var datas in proxy.datas)
                    {
                        datas.Value.RemoveAt(emptyLine[l]);
                    }
                }


                if (splitColumn)
                {
                    for (int k = 1; k < fieldNames.Count; k++)
                    {
                        var splitProxy = new SheetSerializeProxy<object>();
                        splitProxy.sheetName = proxy.sheetName;
                        splitProxy.datas[fieldNames[0]] = proxy.datas[fieldNames[0]];
                        splitProxy.datas["content"] = proxy.datas[fieldNames[k]];
                        var dataBytes = MessagePack.MessagePackSerializer.Serialize(splitProxy);
                        System.IO.File.WriteAllBytes(Setting.GetBinPath(etype) + headInfo.SheetName + headInfo.Fields[k].Name.Replace("t_", "") + "Bean.bytes", dataBytes);
                    }
                }
                else
                {
                    var dataBytes = MessagePack.MessagePackSerializer.Serialize(proxy);
                    System.IO.File.WriteAllBytes(Setting.GetBinPath(etype) + headInfo.SheetName + "Bean.bytes", dataBytes);
                    //System.IO.File.WriteAllText(Setting.GetBinPath(etype) + headInfo.SheetName + "Bean.txt", MessagePack.MessagePackSerializer.SerializeToJson(proxy));
                }
            }
        }

        /// <summary>
        /// 获取Byte数组内有效数据
        /// </summary>
        /// <param name="org"></param>
        /// <param name="validLen"></param>
        /// <returns></returns>
        public static byte[] GetValidData(byte[] org, int validLen)
        {
            if (validLen > org.Length)
            {
                LOGGER.Error("数据异常org:{},valid:{}", org.Length, validLen);
                return org;
            }
            byte[] res = new byte[validLen];
            Array.Copy(org, 0, res, 0, validLen);
            return res;
        }

    }
}
