using System;
using System.IO;
using TMPro;
using UnityEngine;

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
                return Resources.Load<TextAsset>(fileName).bytes; 
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
