/**
 * Auto generated, do not edit it client
 */
using Data.Containers;
using Base;

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

        public void LoadData(byte[] data, ref int offset)
        {
            t_id = XBuffer.ReadInt(data, ref offset);
            t_int_param = XBuffer.ReadInt(data, ref offset);
			t_string_param = XBuffer.ReadString(data, ref offset);
        }

    }
}
