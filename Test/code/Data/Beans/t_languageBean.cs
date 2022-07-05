/**
 * Auto generated, do not edit it client
 * 语言包表
 */
using Data.Containers;
using Base;

namespace Data.Beans
{
    public class t_languageBean : BaseBin
    {

		public int t_id;
		public string t_content;

        public void LoadData(byte[] data, ref int offset)
        {

			string useField = ConfigManager.Singleton.Language;
            t_id = XBuffer.ReadInt(data, ref offset);

			if (string.IsNullOrEmpty(t_content) && useField == "t_chinese")
			{
				t_content = XBuffer.ReadString(data, ref offset); 
			}
			else
			{
				//ignore no need language
				int slen = XBuffer.ReadInt(data, ref offset);
				offset += slen;
			}


			if (string.IsNullOrEmpty(t_content) && useField == "t_chinesetraditional")
			{
				t_content = XBuffer.ReadString(data, ref offset); 
			}
			else
			{
				//ignore no need language
				int slen = XBuffer.ReadInt(data, ref offset);
				offset += slen;
			}


			if (string.IsNullOrEmpty(t_content) && useField == "t_english")
			{
				t_content = XBuffer.ReadString(data, ref offset); 
			}
			else
			{
				//ignore no need language
				int slen = XBuffer.ReadInt(data, ref offset);
				offset += slen;
			}

        }

    }
}
