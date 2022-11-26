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
	[MessagePackObject(true)]
    public class t_monsterBeanDeserializeProxyData
    {

        public List<int> t_id; 

        public List<int> t_name; 

        public List<int> t_monster_id; 

        public List<int> t_monster_ui; 

        public List<int> t_monster_type; 

        public List<int> t_star; 

        public List<int> t_lv; 

        public List<int> t_head; 

        public List<int> t_camp; 

        public List<int> t_type; 

        public List<int> t_battle_prefab; 

        public List<int> t_scale; 

        public List<string> t_skill; 

        public List<int> t_att; 

    }

    [MessagePackObject(true)]
    public class t_monsterBeanDeserializeProxy
    { 
        public string sheetName;   
		public t_monsterBeanDeserializeProxyData datas;
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
					var datas = proxy.datas;
					var rowCount = datas.t_id.Count;
					list = new List<t_monsterBean>(rowCount); 
                    for (int i = 0; i < rowCount; i++)
                    {
                        var bean = new t_monsterBean();
                        list.Add(bean);

						if (datas.t_id != null && datas.t_id.Count > i)
                        { 
							bean.t_id = datas.t_id[i];
                        }

						if (datas.t_name != null && datas.t_name.Count > i)
                        { 

                            bean.m_t_name = datas.t_name[i];
                        }

						if (datas.t_monster_id != null && datas.t_monster_id.Count > i)
                        { 
							bean.t_monster_id = datas.t_monster_id[i];
                        }

						if (datas.t_monster_ui != null && datas.t_monster_ui.Count > i)
                        { 
							bean.t_monster_ui = datas.t_monster_ui[i];
                        }

						if (datas.t_monster_type != null && datas.t_monster_type.Count > i)
                        { 
							bean.t_monster_type = datas.t_monster_type[i];
                        }

						if (datas.t_star != null && datas.t_star.Count > i)
                        { 
							bean.t_star = datas.t_star[i];
                        }

						if (datas.t_lv != null && datas.t_lv.Count > i)
                        { 
							bean.t_lv = datas.t_lv[i];
                        }

						if (datas.t_head != null && datas.t_head.Count > i)
                        { 
							bean.t_head = datas.t_head[i];
                        }

						if (datas.t_camp != null && datas.t_camp.Count > i)
                        { 
							bean.t_camp = datas.t_camp[i];
                        }

						if (datas.t_type != null && datas.t_type.Count > i)
                        { 
							bean.t_type = datas.t_type[i];
                        }

						if (datas.t_battle_prefab != null && datas.t_battle_prefab.Count > i)
                        { 
							bean.t_battle_prefab = datas.t_battle_prefab[i];
                        }

						if (datas.t_scale != null && datas.t_scale.Count > i)
                        { 
							bean.t_scale = datas.t_scale[i];
                        }

						if (datas.t_skill != null && datas.t_skill.Count > i)
                        { 
							bean.t_skill = datas.t_skill[i];
                        }

						if (datas.t_att != null && datas.t_att.Count > i)
                        { 
							bean.t_att = datas.t_att[i];
                        }

                    }

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


