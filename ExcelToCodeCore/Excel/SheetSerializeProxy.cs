using MessagePack;
using System.Collections.Generic;

namespace ExcelToCode.Excel
{
    [MessagePackObject(true)]
    public class SheetSerializeProxy<T> where T : class
    {
        public string sheetName;
        //按列存储的数据
        public Dictionary<string, List<object>> datas = new();
    }
}
