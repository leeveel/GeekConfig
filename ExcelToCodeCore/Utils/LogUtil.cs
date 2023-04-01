using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToCodeCore.Utils
{
    public abstract class AbstractLogUtil
    {
        public abstract void Add(string log, bool isErr = false);

        public void AddIgnoreLog(string fileName, string sheetName, string reason)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(fileName);
            sb.Append("]--");
            sb.Append("[");
            sb.Append(sheetName);
            sb.Append("]--");
            sb.Append(reason);
            sb.Append("--被忽略");
            Add(sb.ToString(), true);
        }

        public void AddNormalLog(string fileName, string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(fileName);
            sb.Append("]--");
            sb.Append(msg);
            Add(sb.ToString());
        }
    }

    public class LogUtil
    {
        private static AbstractLogUtil logUtil;

        public static void Init(AbstractLogUtil logUtil)
        {
            LogUtil.logUtil = logUtil;
        }

        public static void Add(string log, bool isErr = false)
        {
            logUtil?.Add(log, isErr);
        }

        public static void AddIgnoreLog(string fileName, string sheetName, string reason)
        {
            logUtil?.AddIgnoreLog(fileName, sheetName, reason);
        }

        public static void AddNormalLog(string fileName, string msg)
        {
            logUtil?.AddNormalLog(fileName, msg);
        }
    }
}
