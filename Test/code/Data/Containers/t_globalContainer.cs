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
using Data.SelfDefineType;

namespace Data.Containers
{
	[MessagePackObject(true)]
    public class t_globalBeanDeserializeProxyData
    {

        public List<int> t_id; 

        public List<int> t_int_param; 

        public List<string> t_string_param; 

        public List<List<int>> t_array_param; 

        public List<SkillTarget> t_enum_Param; 

        public List<List<SkillTarget>> t_enumArray_Param; 

        public List<TestClass> t_testclass_Param; 

    }

    [MessagePackObject(true)]
    public class t_globalBeanDeserializeProxy
    { 
        public string sheetName;   
		public t_globalBeanDeserializeProxyData datas;
    }

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
					var proxy = MessagePack.MessagePackSerializer.Deserialize<t_globalBeanDeserializeProxy>(data); 
					var datas = proxy.datas;
					var rowCount = datas.t_id.Count;
					list = new List<t_globalBean>(rowCount); 
                    for (int i = 0; i < rowCount; i++)
                    {
                        var bean = new t_globalBean();
                        list.Add(bean);

						if (datas.t_id != null && datas.t_id.Count > i)
                        { 
							bean.t_id = datas.t_id[i];
                        }

						if (datas.t_int_param != null && datas.t_int_param.Count > i)
                        { 
							bean.t_int_param = datas.t_int_param[i];
                        }

						if (datas.t_string_param != null && datas.t_string_param.Count > i)
                        { 
							bean.t_string_param = datas.t_string_param[i];
                        }

						if (datas.t_array_param != null && datas.t_array_param.Count > i)
                        { 
							bean.t_array_param = datas.t_array_param[i];
                        }

						if (datas.t_enum_Param != null && datas.t_enum_Param.Count > i)
                        { 
							bean.t_enum_Param = datas.t_enum_Param[i];
                        }

						if (datas.t_enumArray_Param != null && datas.t_enumArray_Param.Count > i)
                        { 
							bean.t_enumArray_Param = datas.t_enumArray_Param[i];
                        }

						if (datas.t_testclass_Param != null && datas.t_testclass_Param.Count > i)
                        { 
							bean.t_testclass_Param = datas.t_testclass_Param[i];
                        }

                    }

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


