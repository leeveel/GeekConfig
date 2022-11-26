/**
 * Auto generated, do not edit it server
 *
 * 语言包表
 */

using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Data.Beans;
using MessagePack;

namespace Data.Containers
{   
	[MessagePackObject(false)]
    public class t_languageBeanDeserializeProxy
    {
        [Key(0)]
        public string sheetName;
        [Key(1)]
        public List<string> fieldNames = new List<string>();
        [Key(2)]
        public List<t_languageBean> datas = new List<t_languageBean>();
    }

	public class t_languageContainer : BaseContainer
	{
		private List<t_languageBean> list = new List<t_languageBean>();
		private Dictionary<int, t_languageBean> map = new Dictionary<int, t_languageBean>();

		//public override List<t_languageBean> getList()
		public override IList getList()
		{
			return list;
		}

		//public override Dictionary<int, t_languageBean> getMap()
		public override IDictionary getMap()
		{
			return map;
		}
		
		public Type BinType = typeof(t_languageBean);

		public override void loadDataFromBin()
		{    
			map.Clear();
			list.Clear();
			Loaded = true;
			
            string useField = ConfigManager.Singleton.Language;
            byte[] data = ConfigManager.Singleton.GetData($"t_language{useField.Replace("t_", "")}Bean"); 
			if(data != null)
			{
				try
				{
					var proxy = MessagePack.MessagePackSerializer.Deserialize<t_languageBeanDeserializeProxy>(data);
                    list = proxy.datas;
                    foreach (var d in list)
                    {
                        if (!map.ContainsKey(d.t_id))
                            map.Add(d.t_id, d);
                        else
                            Debuger.Err("Exist duplicate Key: " + d.t_id + " t_languageBean");
                    }
				}
				catch (Exception ex)
				{
					Debuger.Err("import data error: t_languageBean >>" + ex.ToString());
				}
			}
			else
			{
				Debuger.Err("can not find conf data: t_languageBean.bytes");
			}
		}
		
	}
}


