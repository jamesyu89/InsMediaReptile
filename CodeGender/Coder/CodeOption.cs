using System.Collections.Generic;

namespace InstagramPhotos.CodeGender.Coder
{
    public class CodeOption
    {
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 带前缀表名
        /// </summary>
        public string FullTableName { get; set; }
        /// <summary>
        /// 命名空间名称
        /// </summary>
        public string NamespaceName { get; set; }

        /// <summary>
        /// 实体命名空间名称
        /// </summary>
        public string ModelNamespaceName { get; set; }

        /// <summary>
        /// ViewModel命名空间名称
        /// </summary>
        public string DtoNamespaceName { get; set; }

        /// <summary>
        /// ViewModel命名空间名称
        /// </summary>
        public string QueryNamespaceName { get; set; }

        /// <summary>
        /// dal命名空间名称
        /// </summary>
        public string DalNamespaceName { get; set; }

        public string DalClassName { get; set; }

        /// <summary>
        /// bll命名空间名称
        /// </summary>
        public string BllNamespaceName { get; set; }

        /// <summary>
        /// 管理类名
        /// </summary>
        public string ManagerClassName { get; set; }

        /// <summary>
        /// 数据列
        /// </summary>
        public List<Column> Columns { get; set; }

        /// <summary>
        /// 所需生成的副键列
        /// </summary>
        public List<Column> AssistantColumn { get; set; }

        /// <summary>
        /// 所需生成的副键列存储过程名集合
        /// </summary>
        public List<string> AssistantColumnProName { get; set; }
        /// <summary>
        /// 所需生成的副键列方法名集合
        /// </summary>
        public List<string> AssistantFunName { get; set; }

        /// <summary>
        /// Id所在数据列
        /// </summary>
        public Column IdColumn { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 获取Id集合存储过程
        /// </summary>
        public string GetEntityIdsStoredProcedureName { get; set; }

        /// <summary>
        /// 根据Id集合获取实体集合
        /// </summary>
        public string GetEntitiesStoredProcedureName { get; set; }

        public string CreateEntityStoredProcedureName { get; set; }

        public string UpdateEntityStoredProcedureName { get; set; }

        public string DeleteEntityStoredProcedureName { get; set; }

        /// <summary>
        /// 是否获得自增主键GUID
        /// </summary>
        public bool GetNewAutoGUID { get; set; }

        public bool EnableParamCache { get; set; }

        public bool ProtoBuf { get; set; }

        public bool WithTrans { get; set; }

        public bool CreateBatOperation { get; set; }

        public bool FilterCode { get; set; }

        public bool IsBllSinglon { get; set; }

        public bool IsPartial { get; set; }

        public string InitDalNameSpace { get; set; }

        public string ParentClass { get; set; }

        public string EntityModelName
        {
            get { return this.ClassName + "Entity"; }
        }
        public string DomainModelName
        {
            get { return this.ClassName + "DO"; }
        }

        public string QueryModelName
        {
            get { return this.ClassName + "QO"; }
        }
    }
}
