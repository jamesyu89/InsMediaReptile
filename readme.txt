1.解决方案目录结构：
00 UI
	此层为Application层所需的静态UI资源，如web静态页、css、images等
01 Application
	此层为应用程序层，如.exe文件、web站点根目录发布的文件等，可添加为多个站点
02 Service
	此层为服务层，下辖4个子层，分别为
	QueryModel------------查询实体
	Service---------------接口服务注册
	Service.Interface-----接口定义
	ViewModel-------------视图模型(供各层之间调用)
03 Business
	业务层实现，目录结构举例如下：
	  Order
		|----Mapping
				|----Extension.cs
		|----OrderCommom.cs
		|----OrderHelper.cs
		|----OrderService.cs
04 Data
	数据访问层，此层包含2个子层，如下：
	DomainModel-------领域模型层，此层是数据库各对象(主要是表)的直接关系映射
	Repository--------数据存取层，此层是与数据库交互的实现，目前支持MSSQL及MySQL
05 Utility
	框架通用中间件层，此层提供解决各种问题的.NET解决方案
06 Tools
	工具层，此层目前只放置代码自动生成的工程，有需要可以自行再添加。
07 Solution Items
	解决方案说明及未尽事项