using DotLiquid;
using ExcelConverter.Utils;
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
            }
        }

        public static void Export(ExportType etype, List<string> fileList, bool isAll)
        {
            Init();
            DataType.Init();

            var tempFileList = new List<string>(fileList.ToArray());

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

            //判断是否有自定义类型表，有的话，先生成类型信息
            var selfDefFile = tempFileList.Find(f => f.Contains("typedefine.xlsx"));
            if (selfDefFile != null)
            {
                ParseSelfDefType(selfDefFile);
                tempFileList.Remove(selfDefFile);
            }

            DataMgrInfo mgrInfo = new DataMgrInfo();
            mgrInfo.Enumtypes = DataType.selfEnumMapper.Values.ToList();
            mgrInfo.Classtypes = DataType.selfClassMapper.Values.ToList();

            for (int i = 0; i < tempFileList.Count; i++)
            {
                ExcelReader excelReader = new ExcelReader();
                ExcelPackage package = null;
                List<SheetHeadInfo> headInfos = excelReader.ReadHeadInfo(fileList[i], etype, out package);

                GenBin(headInfos, package, etype);
                //Task.Run(()=> { GenBin(headInfos, package, etype); });
                GenBeanAddContainer(headInfos, etype, mgrInfo);


                LogUtil.AddNormalLog(package.File.Name, "导表完成");

                if (package != null)
                    package.Dispose();
            }

            GenSelfDef(etype, mgrInfo);
            GenDeserializeProxy(etype);
            GenGameDataManager(mgrInfo, etype, isAll);
            ExcelToCode.Program.MainForm.ToggleAllBtn(true);
            LogUtil.Add("---------导表完成-------------");
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

                string part1 = string.Format("public {0} {1} = new {2}();", containerName, containerName, containerName);
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

        private static void GenDeserializeProxy(ExportType etype)
        {
            string path = Setting.GetCodePath(etype) + @"/Data/SheetDeserializeProxy.cs";
            string templatePath = Setting.GetTemplatePath(etype) + "/SheetDeserializeProxy.template";
            Template template = Template.Parse(File.ReadAllText(templatePath));
            var str = template.Render();
            File.WriteAllText(path, str);
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
                            LogUtil.AddIgnoreLog(tableFile, sheetName, $"未知的类型 {content}");
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
                        }
                        break;
                }
                return value;
            }
        }


        private static void GenBin(List<SheetHeadInfo> headInfos, ExcelPackage package, ExportType etype)
        {
            var msgPackOption = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

            for (int i = 0; i < headInfos.Count; i++)
            {
                SheetHeadInfo headInfo = headInfos[i];
                ExcelWorksheet sheet = package.Workbook.Worksheets[headInfo.SheetId]; //只导出合法表单id的数据 
                                                                                      //空表没有数据
                if (ExcelReader.DataStartRow > sheet.Dimension.End.Row)
                    continue;

                var proxy = new SheetDeserializeProxy<object>();
                proxy.sheetName = headInfos[i].SheetName;
                var fieldNames = new List<string>();
                var datas = new List<List<object>>(sheet.Dimension.End.Row - ExcelReader.DataStartRow);
                proxy.fieldNames = fieldNames;
                proxy.datas = datas;

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

                //写入数据
                for (int m = ExcelReader.DataStartRow, n = sheet.Dimension.End.Row; m <= n; m++)
                {
                    var data = new List<object>();

                    for (int j = 0; j < headInfos[i].Fields.Count; j++)
                    {
                        int col = headInfos[i].Fields[j].Col;
                        Field field = headInfos[i].Fields[j];

                        var content = "";
                        var obj = sheet.GetValue(m, col);
                        if (obj != null)
                            content = obj.ToString();

                        //排除id为0的数据行
                        if (j == 0 && string.IsNullOrEmpty(content))
                        {
                            data = null;
                            break;
                        }

                        data.Add(GetTrueValue(package.File.Name, sheet.Name, content, field.Elementtype, field.IsArray, field.ArraySplitChar));
                    }
                    if (data != null)
                        datas.Add(data);
                }

                if (splitColumn)
                {
                    for (int k = 1; k < headInfo.Fields.Count; k++)
                    {
                        var colDatas = new List<List<object>>();
                        proxy.datas = colDatas;
                        foreach (var d in datas)
                        {
                            colDatas.Add(new List<object> { d[0], d[k] });
                        }
                        var dataBytes = MessagePack.MessagePackSerializer.Serialize(proxy, msgPackOption);
                        System.IO.File.WriteAllBytes(Setting.GetBinPath(etype) + headInfo.SheetName + headInfo.Fields[k].Name.Replace("t_", "") + "Bean.bytes", dataBytes);
                    }
                }
                else
                {
                    var dataBytes = MessagePack.MessagePackSerializer.Serialize(proxy, msgPackOption);
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
