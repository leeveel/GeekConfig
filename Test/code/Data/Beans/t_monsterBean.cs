/**
 * Auto generated, do not edit it client
 */
using Data.Containers;
using Base;
using MessagePack;

namespace Data.Beans 
{
	///<summary>怪物表</summary>
    [MessagePackObject(false)]
    public class t_monsterBean : BaseBin
    {
		///<summary>Id</summary>
        [Key(0)]
        public int t_id;

        [Key(1)]
        public int m_t_name;
		///<summary>名字id</summary>
		[IgnoreMember]
        public string t_name
		{
			get           
			{
				if(m_t_name == 0) 
					return "";
				t_languageBean lanBean = ConfigBean.GetBean<t_languageBean, int>(m_t_name);
				if (lanBean != null)
					return lanBean.t_content;
				else
					return m_t_name.ToString();
			}
		}
		///<summary>对应角色id</summary>
        [Key(2)]
        public int t_monster_id;
		///<summary>战斗ui类型</summary>
        [Key(3)]
        public int t_monster_ui;
		///<summary>类型</summary>
        [Key(4)]
        public int t_monster_type;
		///<summary>星级</summary>
        [Key(5)]
        public int t_star;
		///<summary>等级</summary>
        [Key(6)]
        public int t_lv;
		///<summary>头像id</summary>
        [Key(7)]
        public int t_head;
		///<summary>阵营</summary>
        [Key(8)]
        public int t_camp;
		///<summary>职业类型</summary>
        [Key(9)]
        public int t_type;
		///<summary>预制件</summary>
        [Key(10)]
        public int t_battle_prefab;
		///<summary>预制件缩放</summary>
        [Key(11)]
        public int t_scale;
		///<summary>技能id</summary>
        [Key(12)]
        public string t_skill;
		///<summary>属性加成</summary>
        [Key(13)]
        public int t_att;

    }
}
