using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.DataAccess
{

    public class DeleteEntityDAFunc : Function
    {
        string entityClass;
        string storedProcedureName;
        Column idColumn;
        bool withTrans = false;//是否带SQL事务

        public DeleteEntityDAFunc(string entityClass, string storedProcedureName, Column idColumn)
            : base(
                string.Format("Delete{0}", entityClass)
            , "void", null)
        {
            this.entityClass = entityClass;
            this.storedProcedureName = storedProcedureName;
            this.idColumn = idColumn;
            this.Parameters = new List<FunctionParameter>();
            this.Parameters.Add(new FunctionParameter(idColumn.Name.ToFirstLower(), idColumn.CSTypeName));
        }

        public DeleteEntityDAFunc(string entityClass, string storedProcedureName, Column idColumn, bool withTrans)
            : this(entityClass, storedProcedureName, idColumn)
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
                    
                code.AppendLineFormatWithTabs("new SqlParameter(\"@{0}\", {1})", 2, idColumn.Name, idColumn.Name.ToFirstLower());
                code.AppendLineWithTabs(");", 1);
                code.AppendLine("}");

                return code.ToString();
            }
        }
    }

}
