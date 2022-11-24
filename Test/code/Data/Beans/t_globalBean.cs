/**
 * Auto generated, do not edit it client
 */
using Data.Containers;
using Base;
using MessagePack;

namespace Data.Beans 
{
	///<summary>全局表</summary>
    [MessagePackObject(false)]
    public class t_globalBean : BaseBin
    {
		///<summary>Id</summary>
        [Key(0)]
        public int t_id;
		///<summary>整形</summary>
        [Key(1)]
        public int t_int_param;
		///<summary>字符串</summary>
        [Key(2)]
        public string t_string_param;

    }
}
