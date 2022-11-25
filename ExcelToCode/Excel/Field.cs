using DotLiquid;
using System;

namespace ExcelToCode.Excel
{
    public class Field : Drop
    {
        public int Col;

        public int Row;

        public string Serializeid { set; get; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 数据类型(驼峰命名会被模板拆分为DataType==>Data_Type)
        /// (为保证模板和类属性一致，故用小写)
        /// </summary>
        public string Elementtype { set; get; }
        public string Datatype
        {
            get
            {
                return IsArray ? $"List<{Elementtype}>" : Elementtype;
            }
        }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 是否是数组
        /// </summary>
        public bool IsArray { get; set; } = false;
        /// <summary>
        /// 如果是数组，分割字符串
        /// </summary>

        public string ArraySplitChar { get; set; } = ";";
    }
}
