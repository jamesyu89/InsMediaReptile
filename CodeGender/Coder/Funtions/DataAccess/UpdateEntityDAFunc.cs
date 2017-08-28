using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.DataAccess
{
    public class UpdateEntityDAFunc : Function
    {
        string entityClass;
        string storedProcedureName;
        Column idColumn;
        List<Column> columns;
        bool enableParamCache;

        bool withTrans = false;//是否带SQL事务

        public UpdateEntityDAFunc(string entityClass, string storedProcedureName, List<Column> columns, Column idColumn, bool enableParamCache)
            : base(
                string.Format("Update{0}", entityClass)
            , "void", null)
        {
            this.entityClass = entityClass;
            this.storedProcedureName = storedProcedureName;
            this.columns = columns;
            this.idColumn = idColumn;
            this.Parameters = new List<FunctionParameter>();
            this.Parameters.Add(new FunctionParameter(entityClass.ToFirstLower(), entityClass));
            this.enableParamCache = enableParamCache;
        }

        public UpdateEntityDAFunc(string entityClass, string storedProcedureName, List<Column> columns, Column idColumn, bool enableParamCache, bool withTrans)
        :this(entityClass,storedProcedureName,columns,idColumn,enableParamCache)
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
                code.AppendLineFormatWithTabs("SqlHelper.ExecuteNonQuery(conn, {1}\"{0}\",", 1, storedProcedureName, this.withTrans ? "tran, " : string.Empty);
                    

                if (enableParamCache)
                {
                    #region With paramCache

                    bool first = true;
                    int order = 0;
                    foreach (Column column in columns)
                    {
                        code.Append("\t\t");
                        if (!first)
                            code.Append(",");
                        else
                        {
                            first = false;
                            code.Append(" ");
                        }
                        code.AppendLineFormat("{0}.{1}// {2}.{3}", paramEntity, column.Name == entityClass ? ("_" + column.Name).PadRight(29) : column.Name.PadRight(29), order++, column.Remarks);
                    }

                    #endregion
                }
                else
                {
                    #region Without paramCache

                    bool first = true;
                    foreach (Column column in columns)
                    {
                        code.Append("\t\t");
                        if (!first)
                            code.Append(",");
                        else
                            first = false;
                        code.AppendLineFormat("new SqlParameter(\"@{0}\", {1}.{2})", column.Name, paramEntity,column.Name==entityClass?("_"+column.Name):column.Name );
                    }

                    #endregion
                }
                
                code.AppendLineWithTabs(");", 1);
                code.AppendLine("}");

                return code.ToString();
            }
        }
    }

}
