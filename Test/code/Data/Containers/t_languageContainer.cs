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

namespace Data.Containers
{
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
			
			byte[] data = ConfigManager.Singleton.GetData("t_languageBean");
			if(data != null)
			{
				try
				{
					int offset = 0;
					//filed count��int��+ field type��byte��(0:int 1:long 2:string 3:float)
					offset = 58;  
					while (data.Length > offset)
					{
						t_languageBean bean = new t_languageBean();
						bean.LoadData(data, ref offset);
						list.Add(bean);
						if(!map.ContainsKey(bean.t_id))
							map.Add(bean.t_id, bean);
						else
							Debuger.Err("Exist duplicate Key: " + bean.t_id + " t_languageBean");
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


