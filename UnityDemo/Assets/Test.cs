using System.Collections;
using System.Collections.Generic;
using Base;
using Data.Beans;
using MessagePack;
using MessagePack.Resolvers;
using Resolvers;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Awake()
    {
        StaticCompositeResolver.Instance.Register( 
            BuiltinResolver.Instance,
            AttributeFormatterResolver.Instance,
            MessagePack.Unity.UnityResolver.Instance,
            PrimitiveObjectResolver.Instance, 
            StandardResolver.Instance,
            ConfigDataResolver.Instance
        );
        var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
        MessagePackSerializer.DefaultOptions = option;  
    }
    void Start()
    {
        ConfigManager.Singleton.Language = "t_chinese";

        //设置测试服务器/客户端
        ConfigManager.Singleton.IsServer = false;
        //可以在程序已启动时候把所有数据表加载到内存
        //也可以需要的时候再加载（未处理线程安全，所以服务器应该在启服时全部加载）
        //GameDataManager.Instance.LoadAll();
        var bean = ConfigBean.GetBean<t_globalBean, int>(1020001);
        if (bean != null)
            Debug.Log(bean.t_string_param);

        var globalList = ConfigBean.GetBeanList<t_globalBean>();
        foreach (var item in globalList)
        {
            Debug.Log(item.t_id + "-------" + item.t_array_param.Count + "-------" + item.t_int_param + "-------" + item.t_enum_Param);
            foreach (var e in item.t_enumArray_Param)
                Debug.Log("t_enumArray_Param:" + item.t_id + "-------" + e);

            Debug.Log("t_testclass_Param:" + item.t_id + "-------" + item.t_testclass_Param.z + "-------" + item.t_testclass_Param.lan + "-------" + item.t_testclass_Param.str);
        }

        var list = ConfigBean.GetBeanList<t_monsterBean>();
        foreach (var item in list)
            Debug.Log(item.t_name + "-------" + item.t_skill);

        var lanList = ConfigBean.GetBeanList<t_languageBean>();
        foreach (var item in lanList)
            Debug.Log(item.t_id + "-------" + item.t_content);

        Debug.Log("-----------------end----------------");
    } 
}
