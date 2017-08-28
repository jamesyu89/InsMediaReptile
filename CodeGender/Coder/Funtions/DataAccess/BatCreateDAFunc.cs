using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.DataAccess
{
    public class BatCreateDAFunc : Function
    {
        string entityClass;
        string storedProcedureName;
        Column idColumn;
        List<Column> columns;
        bool enableParamCache;
        bool needAutoGuid;

        bool withTrans = false;//是否带SQL事务

        public BatCreateDAFunc(string entityClass, string storedProcedureName, List<Column> columns, Column idColumn, bool enableParamCache, bool needAutoGuid)
            : base(
                string.Format("Create{0}", entityClass)
            , " void", null)
        {
            this.entityClass = entityClass;
            this.storedProcedureName = storedProcedureName;
            this.columns = columns;
            this.idColumn = idColumn;
            this.Parameters = new List<FunctionParameter>();
            this.Parameters.Add(new FunctionParameter(entityClass.ToFirstLower() + "s", string.Format("List<{0}>", entityClass)));
            this.enableParamCache = enableParamCache;
            this.needAutoGuid = needAutoGuid;
        }

        public BatCreateDAFunc(string entityClass, string storedProcedureName, List<Column> columns, Column idColumn, bool enableParamCache, bool needAutoGuid, bool withTrans)
            : this(entityClass, storedProcedureName, columns, idColumn, enableParamCache, needAutoGuid)
        {
            this.withTrans = withTrans;
            if (withTrans)
                this.Parameters.Add(new FunctionParameter("tran", "SqlTransaction"));
        }
        
        public override string Body
        {
            get
            {
                string paramEntity = entityClass.ToFirstLower();
                StringBuilder code = new StringBuilder();

                code.AppendLine("using (SqlConnection conn = GetSqlConnection())");
                code.AppendLine("{");

                if (enableParamCache)
                {
                    #region With paramcache

                    code.AppendLineFormatWithTabs("SqlHelper.ExecuteNonQuery(conn, {2}\"{0}\",{1});", 1
                        , storedProcedureName
                        , string.Format("SqlHelper.ConvertModelListToXML<{0}>(\"e\", {1})", this.entityClass, this.Parameters[0].Name)
                        , this.withTrans ? "tran, " : string.Empty);

                    #endregion
                }
                else
                {
                    #region Without paramcache

                    code.AppendLineFormatWithTabs("SqlHelper.ExecuteNonQuery(conn, {2}\"{0}\",{1});", 1
                        , storedProcedureName
                        , string.Format("new SqlParameter(\"@models\",SqlHelper.ConvertModelListToXML<{0}>(\"e\", {1}))", this.entityClass, this.Parameters[0].Name)
                        , this.withTrans ? "tran, " : string.Empty);

                    #endregion
                }
                code.AppendLine("}");

                return code.ToString();
            }
        }
    }
}
