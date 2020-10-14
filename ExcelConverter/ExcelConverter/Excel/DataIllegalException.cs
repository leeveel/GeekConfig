using System;

namespace ExcelReader.Excel
{
    public class DataIllegalException : Exception
    {

        public DataIllegalException(string message)
            :base(message)
        {
            
        }

    }
}
