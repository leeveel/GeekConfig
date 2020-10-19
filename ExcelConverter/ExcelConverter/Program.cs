using ExcelConverter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelConverter
{
    static class Program
    {

        public static Form1 MainForm = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //如果需要将工具依赖的DLL都打包进EXE，参考以下网址
            //https://www.cnblogs.com/lip-blog/p/7365942.html
//#if (!DEBUG)
//            DllLoader.Load();
//#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new Form1();
            Application.Run(MainForm);
        }
    }
}
