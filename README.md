# GeekConfig
Excel一键导出二进制数据及解析代码，Super Fast，支持Unity3d，IlRuntime，.Netcore , .Net Framework  
API简洁，使用方便，性能高

### Excel填表规则（请参考output/excel下模板进行填写）
1.第一行：CS代表这张表客户端和服务器都会导出（c:client s:server,不填代表cs）  
2.第二行：字段名称，必须以t_开头（非t_开头列不会被导出）  
3.第三行：数据类型，目前支持如下类型   
- int（不填写默认为int）          
- text   
- long   
- float
- textmult(多语言处理，代表这个字段会从语言表中读取真正的值)
- 自定义枚举(在@typedefine.xlsx enumdef表中定义)    
- 以及上述类型的数组类型(字段填写方式: int[];  类型[]分割符)   
- 自定义结构(在@typedefine.xlsx classdef表中定义,结构基础类型包含以上类型) 

4.第四行：第一列为表名（主键列，一定会导出），后续的列可以通过填c,s,cs,sc来控制是否需要导出（c:client s:server,不填代表cs）  
5.第五行：字段备注  
6.表单名字必须以t_开头（非t_开头表单不会被导出）,支持多个表单  

7.示例   
|cs|  |  |  |   |    |   |   |  |
| -|- | -| -| -| -| - | - | - |
|t_id|t_int_param|t_string_param|t_array_param|t_enum_Param|t_enumArray_Param|t_testclass_Param|    
| | |text|int[];|SkillTarget|SkillTarget[];|TestClass|   
全局表|   
|id|说明1|说明2| | | | |  
|1010001|4|字符串测试|1;2;3;4;5;5;5|自己|敌人;同伴|	x:2 y:3 z:4 lan:1000 str:test字符串 



### 支持按需加载/启动时全部加载（非线程安全）
GameDataManager.Instance.LoadAll();  
建议:服务器一开始就全部加载，客户端（unity3d）单线程按需加载即可

### 按ID获取数据
var bean = ConfigBean.GetBean<t_globalBean, int>(1020001);  
if(bean != null) Console.WriteLine(bean.t_string_param);

### 获取整个列表
var list = ConfigBean.GetBeanList<t_monsterBean>();  
foreach (var item in list)  
    Console.WriteLine(item.t_name + "-------" + item.t_skill);

### 说明
Configs/config.xml: 可以对工具相关路径进行设置  
Configs/Template: 工程中的client和server模板是一样的，可以根据自己的需求修改
![Image text](https://github.com/leeveel/ExcelToCode/blob/main/Doc/configtool.png)
