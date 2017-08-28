using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;
using InstagramPhotos.CodeGender.Helper;

namespace InstagramPhotos.CodeGender.Coder.Funtions.DataAccess
{
    public class GetEntitiesDAFunc : Function
    {
        string entityClass;
        string storedProcedureName;
        string paramIds;
        string paramEntities;
        Column idColumn;

        public GetEntitiesDAFunc(string entityClass, string storedProcedureName, Column idColumn,string FunName)
            : base(
                string.Format("Get{0}", PluralizerHelper.ToPlural(FunName))
            , "void", null)
        {
            this.entityClass = entityClass;
            this.storedProcedureName = storedProcedureName;
            this.idColumn = idColumn;
            paramIds = idColumn.Name.ToFirstLower().ToPlural();
            paramEntities = PluralizerHelper.ToPlural(entityClass.ToFirstLower());
            this.Parameters = new List<FunctionParameter>();
            this.Parameters.Add(new FunctionParameter(paramIds, string.Format("{0}[]", idColumn.CSTypeName)));
            this.Parameters.Add(new FunctionParameter(paramEntities, string.Format("ref Dictionary<{0}, {1}>", idColumn.CSTypeName, entityClass)));
        }

        public override string Body
        {
            get {
                StringBuilder code = new StringBuilder();
                code.AppendLineFormatWithTabs("if ({0} == null)", 0, paramEntities);
                code.AppendLineFormatWithTabs("{0} = new Dictionary<{1}, {2}>();", 1
                    , paramEntities, idColumn.CSTypeName, entityClass);
                code.AppendLine();

                code.AppendLineWithTabs("using (SqlConnection conn = GetSqlConnection())", 0);
                code.AppendLineWithTabs("{", 0);
                code.AppendLineFormatWithTabs("using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, \"{0}\",", 1, storedProcedureName);
                code.AppendLineFormatWithTabs("new SqlParameter(\"@Ids\", SqlHelper.ConvertIdsToXML<{0}>(\"e\", {1}))", 3, idColumn.CSTypeName, paramIds);
                code.AppendLineWithTabs("))", 2);
                code.AppendLineWithTabs("{", 1);
                code.AppendLineWithTabs("while (dr.Read())", 2);
                code.AppendLineWithTabs("{", 2);
                code.AppendLineFormatWithTabs("{0} {1} = new {0}(dr);", 3, entityClass, entityClass.ToFirstLower());
                code.AppendLineFormatWithTabs("{0}[{1}.{2}] = {1};", 3, paramEntities, entityClass.ToFirstLower(), idColumn.Name);
                code.AppendLineWithTabs("}", 2);
                //code.AppendLineWithTabs("dr.Close();", 2);
                //code.AppendLineWithTabs("conn.Close();", 2);
                code.AppendLineWithTabs("}", 1);
                code.AppendLineWithTabs("}", 0);

                return code.ToString();
            }
        }
    }
}
