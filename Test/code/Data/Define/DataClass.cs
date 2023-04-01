/**
 * Auto generated, do not edit it client
 */
using Base;
using System;
using Data.Beans;
using Data.Containers;
using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace Data.SelfDefineType
{ 
	[MessagePackObject(true)]
	public class TestClass
	{ 

        public float x;

        public float y;

        public float z;

        public string str;

        public int m_lan; 
		[IgnoreMember]
        public string lan
		{
			get           
			{
				if(m_lan == 0) 
					return "";
				t_languageBean lanBean = ConfigBean.GetBean<t_languageBean, int>(m_lan);
				if (lanBean != null)
					return lanBean.t_content;
				else
					return m_lan.ToString();
			}
		}

	}
}