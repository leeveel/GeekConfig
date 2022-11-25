using DotLiquid;
using Microsoft.VisualBasic.Devices;
using NLog;
using NLog.Fluent;
using System.IO.Packaging;
using System.Text.RegularExpressions;

namespace ExcelToCode.Excel
{
    public class EnumTypeField : Drop
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class EnumType : Drop
    {
        public EnumType()
        {
            AddField("NONE");
        }
        public string Name { get; set; }
        public List<EnumTypeField> Fields { get; set; } = new();
        public Dictionary<string, EnumTypeField> FieldsMap { get; set; } = new();

        public void AddField(string name)
        {
            var field = new EnumTypeField { Name = name, Value = Fields.Count };
            Fields.Add(field);
            FieldsMap[name] = field;
        }
    }


    public class ClassType : Drop
    {
        public string Name { get; set; }
        public List<Field> Fields { get; set; } = new();  //序列化的失火用字典，可以保留字段
        public Dictionary<string, Field> FieldsMap { get; set; } = new();

        public string AddField(string name, string type)
        {
            var field = new Field { Name = name, Elementtype = type };
            if (DataType.IsLegal(type))
            {
                var typeinfo = DataType.GetTrueTyped(type);
                field.Elementtype = typeinfo.Item1;
                field.IsArray = typeinfo.Item2;
                field.ArraySplitChar = typeinfo.Item3;
            }
            else
            {
                return $"自定义类{Name},未知的数据类型{type}";
            }

            Fields.Add(field);
            FieldsMap[name] = field;
            return null;
        }
    }

    public static class DataType
    {

        public const string Int = "int";

        public const string Long = "long";

        public const string Float = "float";

        public const string Text = "text";

        public const string TextMult = "textmult";

        public static Dictionary<string, string> selfDefineType = new();

        static Dictionary<string, string> columnTypeMapper;

        public static Dictionary<string, EnumType> selfEnumMapper;

        public static Dictionary<string, ClassType> selfClassMapper;

        public static void Init()
        {
            columnTypeMapper = new Dictionary<string, string>
            {
                {"","int" },
                {Int,"int" },
                {Long,"long" },
                {Float,"float" },
                {Text,"string" },
                {TextMult,TextMult },
            };
            selfEnumMapper = new();
            selfClassMapper = new();
        }

        public static void AddEnum(EnumType t)
        {
            selfEnumMapper.Add(t.Name, t);
            columnTypeMapper[t.Name] = t.Name;
        }

        public static void AddClass(ClassType t)
        {
            selfClassMapper.Add(t.Name, t);
            columnTypeMapper[t.Name] = t.Name;
        }

        public static bool IsEnum(string type)
        {
            return selfEnumMapper.ContainsKey(type);
        }

        public static bool IsClass(string type)
        {
            return selfClassMapper.ContainsKey(type);
        }

        public static ClassType GetClassType(string type)
        {
            return selfClassMapper[type];
        }

        public static int GetEnumValue(string enumName, string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return 0;
            }
            var fmap = selfEnumMapper[enumName].FieldsMap;
            if (fmap.TryGetValue(fieldName, out var v))
            {
                return v.Value;
            }
            else
            {
                return -1;
            }
        }

        public static bool IsLegal(string type)
        {
            if (type.Contains("[]"))
            {
                var t = type.Split("[]")[0];
                return IsLegal(t);
            }
            return columnTypeMapper.ContainsKey(type);
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
            var trueType = columnTypeMapper[type];
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
