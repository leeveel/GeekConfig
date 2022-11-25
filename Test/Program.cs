using Base;
using Data.Beans;
using Data.Containers;
using MessagePack;
using System;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----------------start----------------");

            ConfigManager.Singleton.Language = "t_chinese";

            //设置测试服务器/客户端
            ConfigManager.Singleton.IsServer = false;
            //可以在程序已启动时候把所有数据表加载到内存
            //也可以需要的时候再加载（未处理线程安全，所以服务器应该在启服时全部加载）
            //GameDataManager.Instance.LoadAll();
            var bean = ConfigBean.GetBean<t_globalBean, int>(1020001);
            if (bean != null)
                Console.WriteLine(bean.t_string_param);

            var globalList = ConfigBean.GetBeanList<t_globalBean>();
            foreach (var item in globalList)
            {
                Console.WriteLine(item.t_id + "-------" + item.t_array_param.Count + "-------" + item.t_int_param + "-------" + item.t_enum_Param);
                foreach (var e in item.t_enumArray_Param)
                    Console.WriteLine(item.t_id + "-------" + e);
            }

            var list = ConfigBean.GetBeanList<t_monsterBean>();
            foreach (var item in list)
                Console.WriteLine(item.t_name + "-------" + item.t_skill);

            var lanList = ConfigBean.GetBeanList<t_languageBean>();
            foreach (var item in lanList)
                Console.WriteLine(item.t_id + "-------" + item.t_content);

            Console.WriteLine("-----------------end----------------");
            Console.ReadLine();
        }
    }
}
