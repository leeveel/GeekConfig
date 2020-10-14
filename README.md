# ExcelToCode
Excel一键导出二进制数据及解析代码，Super Fast，支持Unity3d，IlRuntime，.Netcore , .Net Framework

# 支持按需加载/启动时全部加载（非线程安全）
GameDataManager.Instance.LoadAll();
Base Practice:服务器一开始就全部加载，客户端（unity3d）单线程按需加载即可

# 按ID获取数据
var bean = ConfigBean.GetBean<t_globalBean, int>(1020001);
if(bean != null) Console.WriteLine(bean.t_string_param);

# 获取整个列表
var list = ConfigBean.GetBeanList<t_monsterBean>();
foreach (var item in list)
    Console.WriteLine(item.t_name + "-------" + item.t_skill);
    
    
