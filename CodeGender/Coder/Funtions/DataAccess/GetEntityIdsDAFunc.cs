using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.DataAccess
{
    public class GetEntityIdsDAFunc : Function
    {
        string entityClass;
        string storedProcedureName;
        Column idColumn;

        public GetEntityIdsDAFunc(string entityClass, string storedProcedureName, Column idColumn)
            : base(
                string.Format("Get{0}{1}", entityClass, idColumn.Name.ToPlural())
                , string.Format("List<{0}>", idColumn.CSTypeName)
                , null)
        {
            this.entityClass = entityClass;
            this.storedProcedureName = storedProcedureName;
            this.idColumn = idColumn;
        }

        public override string Body
        {
            get {
                string paramIds = string.Format("{0}", idColumn.Name.ToFirstLower().ToPlural());
                
                StringBuilder code = new StringBuilder();
                code.AppendLineFormatWithTabs("List<{0}> {1} = null;", 0, idColumn.CSTypeName, paramIds);
                code.AppendLineWithTabs("using (SqlConnection conn = GetSqlConnection())", 0);
                code.AppendLineWithTabs("{", 0);
                code.AppendLineFormatWithTabs("using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, \"{0}\",null))", 1, storedProcedureName);
                code.AppendLineWithTabs("{", 1);
                code.AppendLineFormatWithTabs(" {0} = SqlHelper.PopulateReadersToIds<{1}>(dr, \"{2}\");", 2, paramIds, idColumn.CSTypeName, idColumn.Name);
                code.AppendLineWithTabs("}", 1);
                code.AppendLineWithTabs("}", 0);
                code.AppendLineFormatWithTabs("return {0};", 0, paramIds);

                return code.ToString();
            }
        }

    }
}
