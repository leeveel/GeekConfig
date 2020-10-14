using Base;
using Data.Beans;
using Data.Containers;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----------------start----------------");
            //设置测试服务器/客户端
            ConfigManager.Singleton.IsServer = false;
            //可以在程序已启动时候把所有数据表加载到内存
            //也可以需要的时候再加载（未处理线程安全，所以服务器应该在启服时全部加载）
            //GameDataManager.Instance.LoadAll();
            var bean = ConfigBean.GetBean<t_globalBean, int>(1020001);
            if(bean != null)
                Console.WriteLine(bean.t_string_param);

            Console.WriteLine("-----------------end----------------");
            Console.ReadLine();
        }
    }
}
