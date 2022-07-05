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
					int offset = 0;
					//filed count��int��+ field type��byte��(0:int 1:long 2:string 3:float)
					offset = 48;  
					while (data.Length > offset)
					{
						t_globalBean bean = new t_globalBean();
						bean.LoadData(data, ref offset);
						list.Add(bean);
						if(!map.ContainsKey(bean.t_id))
							map.Add(bean.t_id, bean);
						else
							Debuger.Err("Exist duplicate Key: " + bean.t_id + " t_globalBean");
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


