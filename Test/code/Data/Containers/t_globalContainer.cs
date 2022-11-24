/**
 * Auto generated, do not edit it server
 *
 * 全局表
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
	public class t_globalContainer : BaseContainer
	{ 
		private List<t_globalBean> list = new List<t_globalBean>();
		private Dictionary<int, t_globalBean> map = new Dictionary<int, t_globalBean>();

		//public override List<t_globalBean> getList()
		public override IList getList()
		{
			return list;
		}

		//public override Dictionary<int, t_globalBean> getMap()
		public override IDictionary getMap()
		{
			return map;
		}
		
		public Type BinType = typeof(t_globalBean);

		public override void loadDataFromBin()
		{    
			map.Clear();
			list.Clear();
			Loaded = true;
			
			byte[] data = ConfigManager.Singleton.GetData("t_globalBean");
			if(data != null)
			{
				try
				{
					var proxy = MessagePack.MessagePackSerializer.Deserialize<SheetDeserializeProxy<t_globalBean>>(data,MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray));
                    list = proxy.datas;
                    foreach (var d in list)
                    {
                        if (!map.ContainsKey(d.t_id))
                            map.Add(d.t_id, d);
                        else
                            Debuger.Err("Exist duplicate Key: " + d.t_id + " t_globalBean");
                    }
				}
				catch (Exception ex)
				{
					Debuger.Err("import data error: t_globalBean >>" + ex.ToString());
				}
			}
			else
			{
				Debuger.Err("can not find conf data: t_globalBean.bytes");
			}
		}
		
	}
}


