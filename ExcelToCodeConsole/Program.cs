using ExcelConverter.Utils;
using ExcelToCode;
using ExcelToCode.Excel;
using ExcelToCodeCore.Utils;

namespace ExcelToCodeConsole
{
    internal class Program
    {
        private static List<string> fileList = null;
        //配置表路径
        private static string sourcePath = "";
        //项目路径
        private static string targetPath = "";

        private static string coverSourcePath = "";

        static async Task<bool> putSetMode(string[] args)
        {
            var argList = new List<string>(args);
            if (!argList.Contains("setMode"))
                return false;

            var argMap = new Dictionary<string, string>();
            foreach (var arg in args)
            {
                var arr = arg.Split('=');
                if (arr.Length >= 2)
                    argMap[arr[0]] = arr[1];
            }

            var output = "client+server";
            foreach (var kv in argMap)
            {
                switch (kv.Key)
                {
                    case "input"://输入目录
                        Setting.ConfigPath = kv.Value;
                        break;
                    case "cover": //差异目录
                        //可以给文件夹名也可以完整路径(完整路径需要和sourcePath同目录)
                        if (kv.Value.Contains(System.IO.Path.DirectorySeparatorChar))
                            Setting.CoverConfigPath = kv.Value;
                        else
                            Setting.CoverConfigPath = System.IO.Path.GetDirectoryName(Setting.ConfigPath) + System.IO.Path.DirectorySeparatorChar + kv.Value;
                        break;
                    case "server-script"://服务器脚本输出目录
                        Setting.ServerCodePath = kv.Value;
                        break;
                    case "server-bin"://服务器bin输出目录
                        Setting.ServerBinPath = kv.Value;
                        break;
                    case "client-script"://客户端脚本输出目录
                        Setting.ClientCodePath = kv.Value;
                        break;
                    case "client-bin"://客户端bin输出目录
                        Setting.ClientBinPath = kv.Value;
                        break;
                    case "output"://导表方式
                        output = kv.Value;
                        break;
                    default:
                        continue;
                }
            }

            List<string> coverFileList = null;
            if (!string.IsNullOrEmpty(Setting.CoverConfigPath))
                coverFileList = FileUtil.GetFileList(Setting.CoverConfigPath, false, ".xlsx");
            fileList = FileUtil.GetFileList(Setting.ConfigPath, false, ".xlsx");

            var startTime = TimeUtils.CurrentTimeMillis();
            if (output.Contains("server"))
            {
                LogUtil.Add("服务器开始");
                var ret = await ExportHelper.Export(ExportType.Server, fileList, coverFileList, true);
                if (!ret)
                    return ret;
                LogUtil.Add("server bin目录：" + System.IO.Path.GetFullPath(Setting.ServerBinPath), true);
                LogUtil.Add("server code目录：" + System.IO.Path.GetFullPath(Setting.ServerCodePath), true);
            }
            if (output.Contains("client"))
            {
                LogUtil.Add("客户端开始");
                var ret = await ExportHelper.Export(ExportType.Client, fileList, coverFileList, true);
                if (!ret)
                    return ret;
                LogUtil.Add("client bin目录：" + System.IO.Path.GetFullPath(Setting.ClientBinPath), true);
                LogUtil.Add("client code目录：" + System.IO.Path.GetFullPath(Setting.ClientCodePath), true);
            }
            if (coverFileList != null)
                LogUtil.Add("差异目录：" + coverSourcePath);
            LogUtil.Add("导表耗时：" + (TimeUtils.CurrentTimeMillis() - startTime));

            return true;
        }

        static async Task Run(string[] args)
        {
            if (await putSetMode(args))
                return;
            if (args.Length < 2)
            {
                Console.WriteLine("参数不正确，参数1为配置表路径，参数2位配置表转Bytes文件路径");
                return;
            }
            sourcePath = args[0];
            targetPath = args[1];

            if (args.Length > 2)
                coverSourcePath = args[2];
            //可以给文件夹名也可以完整路径(完整路径需要和sourcePath同目录)
            if (coverSourcePath != "" && !coverSourcePath.Contains(System.IO.Path.DirectorySeparatorChar))
                coverSourcePath = System.IO.Path.GetDirectoryName(sourcePath) + System.IO.Path.DirectorySeparatorChar + coverSourcePath;

            if (coverSourcePath != null)
                LogUtil.Add("差异目录：" + coverSourcePath);

            Setting.ConfigPath = sourcePath;
            Setting.CoverConfigPath = coverSourcePath;
            Setting.ServerBinPath = targetPath + "/Server/Bytes/";
            Setting.ServerCodePath = targetPath + "/Server/Configs/";

            //读取配置表路径类型并显示
            fileList = FileUtil.GetFileList(sourcePath, false, ".xlsx");

            if (fileList == null || fileList.Count <= 0)
            {
                LogUtil.Add("没有发现配置表");
                return;
            }

            List<string> coverFileList = null;
            if (!string.IsNullOrEmpty(coverSourcePath))
                coverFileList = FileUtil.GetFileList(coverSourcePath, false, ".xlsx");


            var startTime = TimeUtils.CurrentTimeMillis();
            var ret = await ExportHelper.Export(ExportType.Server, fileList, coverFileList, true);

            LogUtil.Add($"导表{(ret ? "成功" : "失败")},耗时{((TimeUtils.CurrentTimeMillis() - startTime) * 0.001).ToString("f2")}秒", !ret);
        }

        static void Main(string[] args)
        {
            LogUtil.Init(new ConsoleLogUtil());
            Run(args).Wait();
        }
    }
}