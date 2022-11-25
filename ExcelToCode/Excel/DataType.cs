using Microsoft.VisualBasic.Devices;
using NLog.Fluent;
using System.Text.RegularExpressions;

namespace ExcelToCode.Excel
{
    public static class DataType
    {

        public const string Int = "int";

        public const string Long = "long";

        public const string Float = "float";

        public const string Text = "text";

        public const string TextMult = "textmult";

        static Dictionary<string, string> columnBaseTypeMapper = new Dictionary<string, string>
        {
            {"","int" },//默认为int
            {Int,"int" },
            {Long,"long" },
            {Float,"float" },
            {Text,"string" },
            {TextMult,TextMult },
        };

        public static bool IsLegal(string type)
        {
            if (type.Contains("[]"))
            {
                var t = type.Split("[]")[0];
                return IsLegal(t);
            }
            return columnBaseTypeMapper.ContainsKey(type);
        }

        //返回参数类型，是否是数组，数组分割符
        public static (string, bool, string) GetTrueTyped(string type, string parentType = null)
        {
            var ret = ("", false, ";");
            if (type.Contains("[]"))
            {
                var strs = type.Split("[]");
                var t = strs[0];
                var splitStr = strs.Length > 1 ? strs[1] : ";";
                var ret1 = GetTrueTyped(t, "List");
                ret.Item1 = ret1.Item1;
                ret.Item2 = ret1.Item2;
                ret.Item3 = splitStr;
                return ret;
            }
            var trueType = columnBaseTypeMapper[type];
            if (parentType == null)
            {
                return (trueType, false, "");
            }
            else if (parentType == "List")
            {
                trueType = trueType == TextMult ? "int" : trueType;
                return (trueType, true, "");
            }
            throw new Exception($"错误的字段类型:{type} {parentType}");
        }

    }
}
