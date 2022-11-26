/**
 * Auto generated, do not edit it client
 */
using Data.Containers;
using Base;
using MessagePack;
using System.Collections.Generic;
using Data.SelfDefineType;

namespace Data.Beans 
{
	///<summary>全局表</summary> 
    public class t_globalBean : BaseBin
    {
		///<summary>Id</summary> 
        public int t_id;
		///<summary>整形</summary> 
        public int t_int_param;
		///<summary>字符串</summary> 
        public string t_string_param;
		///<summary>字符串</summary> 
        public List<int> t_array_param;
		///<summary></summary> 
        public SkillTarget t_enum_Param;
		///<summary></summary> 
        public List<SkillTarget> t_enumArray_Param;
		///<summary></summary> 
        public TestClass t_testclass_Param;

    }
}
