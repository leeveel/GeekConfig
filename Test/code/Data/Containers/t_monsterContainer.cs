/**
 * Auto generated, do not edit it server
 *
 * 怪物表
 */

using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Data.Beans;
using MessagePack;
using Data.SelfDefineType;

namespace Data.Containers
{
    [MessagePackObject(false)]
    public class t_monsterBeanDeserializeProxy
    {
        [Key(0)]
        public string sheetName;
        [Key(1)]
        public List<string> fieldNames = new List<string>();
        [Key(2)]
        public List<t_monsterBean> datas = new List<t_monsterBean>();
    }

	public class t_monsterContainer : BaseContainer
	{ 
		private List<t_monsterBean> list = new List<t_monsterBean>();
		private Dictionary<int, t_monsterBean> map = new Dictionary<int, t_monsterBean>();

		//public override List<t_monsterBean> getList()
		public override IList getList()
		{
			return list;
		}

		//public override Dictionary<int, t_monsterBean> getMap()
		public override IDictionary getMap()
		{
			return map;
		}
		
		public Type BinType = typeof(t_monsterBean);

		public override void loadDataFromBin()
		{    
			map.Clear();
			list.Clear();
			Loaded = true;
			
			byte[] data = ConfigManager.Singleton.GetData("t_monsterBean");
			if(data != null)
			{
				try
				{
					var proxy = MessagePack.MessagePackSerializer.Deserialize<t_monsterBeanDeserializeProxy>(data);
                    list = proxy.datas;
                    foreach (var d in list)
                    {
                        if (!map.ContainsKey(d.t_id))
                            map.Add(d.t_id, d);
                        else
                            Debuger.Err("Exist duplicate Key: " + d.t_id + " t_monsterBean");
                    }
				}
				catch (Exception ex)
				{
					Debuger.Err("import data error: t_monsterBean >>" + ex.ToString());
				}
			}
			else
			{
				Debuger.Err("can not find conf data: t_monsterBean.bytes");
			}
		}
		
	}
}


