using System;
using System.IO;

namespace Base
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigManager : SingletonTemplate<ConfigManager>
    {

        public string Language = "t_chinese";

        public bool IsServer = true;

        public byte[] GetData(string fileName)
        {
            try
            {
                if(IsServer)
                    return File.ReadAllBytes("../../data/server/" + fileName + ".bytes");
                else
                    return File.ReadAllBytes("../../data/client/" + fileName + ".bytes");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
