using System;
using System.Collections.Generic;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.Manager
{
    public class BatCreateFunc : Function
    {
        string entityClass;
        string dataAccessName;
        Column idColumn;
        bool enableParamCache;
        bool needAutoGuid;

        bool withTrans = false;//是否带SQL事务

        public BatCreateFunc(string entityClass, string dataAccessName, Column idColumn, bool enableParamCache, bool needAutoGuid)
            : base(
                string.Format("static Create{0}", entityClass)
            , " void", null)
        {
            this.entityClass = entityClass;
            this.dataAccessName = dataAccessName;
            this.idColumn = idColumn;
            this.Parameters = new List<FunctionParameter>();
            this.Parameters.Add(new FunctionParameter(entityClass.ToFirstLower() + "s", string.Format("List<{0}>", entityClass)));
            this.enableParamCache = enableParamCache;
            this.needAutoGuid = needAutoGuid;
        }

        public BatCreateFunc(string entityClass, string dataAccessName, Column idColumn, bool enableParamCache, bool needAutoGuid, bool withTrans)
            : this(entityClass, dataAccessName, idColumn, enableParamCache, needAutoGuid)
        {
            this.withTrans = withTrans;
            if (withTrans)
                this.Parameters.Add(new FunctionParameter("tran", "TransactionManager"));
        }

        public override string Body
        {
            get { throw new NotImplementedException(); }
        }
    }
}
