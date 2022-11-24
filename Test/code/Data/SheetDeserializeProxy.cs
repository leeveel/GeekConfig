/**
 * Auto generated, do not edit it client
 */
using MessagePack;
using System.Collections.Generic;

namespace Data.Containers
{
    [MessagePackObject(false)]
    public class SheetDeserializeProxy<T> where T : class
    {
        [Key(0)]
        public string sheetName;
        [Key(1)]
        public List<string> fieldNames = new List<string>();
        [Key(2)]
        public List<T> datas = new List<T>();
    }
}