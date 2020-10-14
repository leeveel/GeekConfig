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

            //1.可以在程序已启动时候把所有数据表加载到内存
            //2.也可以需要的时候再加载（未处理线程安全）
            //3.Base Practice:服务器一开始就全部加载，客户端（unity3d）单线程按需加载即可
            //GameDataManager.Instance.LoadAll();

            //获取某个id的数据
            var bean = ConfigBean.GetBean<t_globalBean, int>(1020001);
            if(bean != null)
                Console.WriteLine(bean.t_string_param);

            //获取整个列表
            var list = ConfigBean.GetBeanList<t_monsterBean>();
            foreach (var item in list)
            {
                Console.WriteLine(item.t_name + "-------" + item.t_skill);                
            }

            Console.WriteLine("-----------------end----------------");
            Console.ReadLine();
        }
    }
}
