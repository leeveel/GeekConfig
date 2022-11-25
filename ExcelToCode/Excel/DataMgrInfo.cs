using DotLiquid;
using System.Collections.Generic;

namespace ExcelToCode.Excel
{
    public class DataMgrInfo : Drop
    {
        public List<string> Containers { get; set; }
        public List<EnumType> Enumtypes { get; set; }
        public List<ClassType> Classtypes { get; set; }

        public DataMgrInfo()
        {
            Containers = new List<string>();
        }

    }
}
