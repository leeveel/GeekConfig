using MessagePack;
using System.Collections.Generic;

namespace ExcelToCode.Excel
{
    [MessagePackObject(false)]
    public class SheetDeserializeProxy<T> where T : class
    {
        [Key(0)]
        public string sheetName;
        [Key(1)]
        public List<string> fieldNames = new List<string>();
        [Key(2)]
        public List<List<object>> datas = new List<List<object>>();
    }
}
