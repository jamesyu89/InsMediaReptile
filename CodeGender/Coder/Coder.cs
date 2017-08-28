using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;
using InstagramPhotos.CodeGender.Helper;

namespace InstagramPhotos.CodeGender.Coder
{
    public static partial class Coder
    {
        public static string GenerateEntity(List<Column> columns
            , string nameSpace, string className)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Data;");
            code.AppendLine();
            if (!string.IsNullOrEmpty(nameSpace))
            {
                code.AppendLineFormatWithTabs("namespace {0}", 0, nameSpace);
                code.AppendLine("{");
            }
            code.AppendLineWithTabs("/// <summary>", 1);
            code.AppendLineFormatWithTabs("/// {0}", 1, className);
            code.AppendLineWithTabs("/// </summary>", 1);
            code.AppendLineFormatWithTabs("public class {0}", 1, className);
            code.AppendLineWithTabs("{", 1);

            code.AppendLine();
            code.AppendLineWithTabs("/// <summary>", 2);
            code.AppendLineFormatWithTabs("/// {0} 构造函数", 2, className);
            code.AppendLineWithTabs("/// </summary>", 2);
            code.AppendLineFormatWithTabs("public {0}()", 2, className);
            code.AppendLineWithTabs("{}", 2);
            code.AppendLine();

            code.AppendLine();
            code.AppendLineWithTabs("/// <summary>", 2);
            code.AppendLineFormatWithTabs("/// {0} OR映射构造函数", 2, className);
            code.AppendLineWithTabs("/// </summary>", 2);
            code.AppendLineFormatWithTabs("public {0}(System.Data.IDataReader dr)", 2, className);
            code.AppendLineWithTabs("{", 2);
            foreach (Column column in columns)
            {
                string convert = string.Format(column.ConvertFormat,
                    string.Format("dr[\"{0}\"]", column.Name));
                code.AppendLineFormatWithTabs("this.{0} = {1};", 3, column.Name, convert);
            }
            code.AppendLineWithTabs("}", 2);
            code.AppendLine();


            foreach (Column column in columns)
            {
                code.AppendLine();
                code.AppendLineWithTabs("/// <summary>", 2);
                code.AppendLineFormatWithTabs("/// {0}", 2, column.Remarks);
                code.AppendLineWithTabs("/// </summary>", 2);
                code.AppendLineFormatWithTabs("public {0} {1} {{ get; set; }}", 2, column.CSTypeName, column.Name);
            }

            code.AppendLineWithTabs("}", 1);
            if (!string.IsNullOrEmpty(nameSpace))
            {
                code.AppendLine("}");
            }
            return code.ToString();
        }

        #region DataAccess Code
        public static string GenerateDataAccessOfGetIds(string entityClass, string storedProcedureName, Column idColumn)
        {
            string paramIds = idColumn.Name.ToFirstLower().ToPlural();
            StringBuilder code = new StringBuilder();
            code.AppendLineFormat("public void Get{0}()", idColumn.Name.ToFirstLower().ToPlural());
            code.AppendLine("{");
            code.AppendLineFormatWithTabs("List<{0}> {1} = null;", 1, idColumn.CSTypeName, paramIds);
            code.AppendLineWithTabs("using (SqlConnection conn = GetSqlConnection())", 1);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, \"{0}\"))", 2, storedProcedureName);
            code.AppendLineWithTabs("{", 2);
            code.AppendLineFormatWithTabs(" {0} = SqlHelper.PopulateReadersToIds<{1}>(dr, \"{2}\");", 3, paramIds, idColumn.CSTypeName, idColumn.Name);
            code.AppendLineWithTabs("}", 2);
            code.AppendLineWithTabs("}", 1);
            code.AppendLineFormatWithTabs("return {0};", 1, paramIds);
            code.AppendLine("}");
            return code.ToString();
        }

        public static string GenerateDataAccessOfGetEntities(string entityClass, string storedProcedureName, Column idColumn)
        {
            string paramIds = idColumn.Name.ToFirstLower().ToPlural();
            string paramEntities = PluralizerHelper.ToPlural(entityClass.ToFirstLower());

            StringBuilder code = new StringBuilder();
            //code.AppendLineWithTabs("/// <summary>", 0);
            //code.AppendLineFormatWithTabs("/// 填充{0}集合", 0, entityClass);
            //code.AppendLineWithTabs("/// </summary>", 0);
            //code.AppendLineFormatWithTabs("/// <param name=\"{0}\"></param>", 0, paramIds);
            //code.AppendLineFormatWithTabs("/// <param name=\"{0}\"></param>", 0, paramEntities);
            code.AppendLineFormatWithTabs("public void Get{0}({2}[] {1}, ref Dictionary<{2}, {3}> {4})", 0
                , PluralizerHelper.ToPlural(entityClass), paramIds
                , idColumn.CSTypeName, entityClass, paramEntities);
            code.AppendLineWithTabs("{", 0);

            code.AppendLineFormatWithTabs("if ({0} == null)", 1, paramEntities);
            code.AppendLineFormatWithTabs("{0} = new Dictionary<{1}, {2}>();", 2
                , paramEntities, idColumn.CSTypeName, entityClass);
            code.AppendLine();

            code.AppendLineWithTabs("using (SqlConnection conn = GetSqlConnection())", 1);
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, \"{0}\",", 2, storedProcedureName);
            code.AppendLineFormatWithTabs("new SqlParameter(\"@Ids\", SqlHelper.ConvertIdsToXML<{0}>(\"e\", {1}))", 4, idColumn.CSTypeName, paramIds);
            code.AppendLineWithTabs("))", 3);
            code.AppendLineWithTabs("{", 2);
            code.AppendLineWithTabs("while (dr.Read())", 3);
            code.AppendLineWithTabs("{", 3);
            code.AppendLineFormatWithTabs("{0} {1} = new {0}(dr);", 4, entityClass, entityClass.ToFirstLower());
            code.AppendLineFormatWithTabs("{0}[{1}.{2}] = {1};", 4, paramEntities, entityClass.ToFirstLower(), idColumn.Name);
            code.AppendLineWithTabs("}", 3);
            code.AppendLineWithTabs("dr.Close();", 3);
            code.AppendLineWithTabs("conn.Close();", 3);
            code.AppendLineWithTabs("}", 2);
            code.AppendLineWithTabs("}", 1);

            code.AppendLineWithTabs("}", 0);

            return code.ToString();
        }
        #endregion

        #region ManagerCode

        #endregion

        #region KVStoreManager Code

        public static string GenerateKVStoreManagerCode(string entityClass, Column idColumn)
        {
            StringBuilder code = new StringBuilder();
            string paramEntities = PluralizerHelper.ToPlural(entityClass.ToFirstLower()) + "Cache";
            code.AppendLineFormatWithTabs("static KVStoreEntityTable<{0}, {1}> {2} = KVStoreManager.GetKVStoreEntityTable<{0}, {1}>(\"{2}\", 180, 10000);", 1
                , idColumn.CSTypeName, entityClass, paramEntities);
            code.AppendLineFormatWithTabs("public static KVStoreEntityTable<{0}, {1}> {2}", 1
                , idColumn.CSTypeName, entityClass, paramEntities.ToFirstUpper());
            code.AppendLineWithTabs("{", 1);
            code.AppendLineFormatWithTabs("get {{ return {0}; }}", 2, paramEntities);
            code.AppendLineFormatWithTabs("set {{ {0} = value; }}", 2, paramEntities);
            code.AppendLineWithTabs("}", 1);
            return code.ToString();
        }

        #endregion

        #region Helper
        public static string ConvertTablenameToClassname(string tablename)
        {
            string entityName, prefix;
            GetEntitynameAndPrefixByTablename(tablename, out entityName, out prefix);
            return entityName;
        }

        //
        public static string GetTableNamePrefix(string tablename)
        {
            string entityName, prefix;
            GetEntitynameAndPrefixByTablename(tablename, out entityName, out prefix);
            return prefix.TrimEnd('_');
        }

        public static string ConverTablenameToGetIdsStoredProcedureName(string tablename, Column idColumn)
        {
            if (idColumn == null)
                return string.Empty;

            string entityName, prefix;
            GetEntitynameAndPrefixByTablename(tablename, out entityName, out prefix);
            return string.Format("up_{0}{2}_Get{1}", prefix, idColumn.Name.ToPlural(), entityName);
        }

        public static string ConverTablenameToGetEntitiesStoredProcedureName(string tablename)
        {
            string entityName, prefix;
            GetEntitynameAndPrefixByTablename(tablename, out entityName, out prefix);
            return string.Format("up_{0}{2}_Get{1}", prefix, PluralizerHelper.ToPlural(entityName), entityName);
        }

        public static string ConverTablenameToActEntityStoredProcedureName(string tablename, string action)
        {
            string entityName, prefix;
            GetEntitynameAndPrefixByTablename(tablename, out entityName, out prefix);
            return string.Format("up_{0}{1}_{2}", prefix, entityName, action);
        }

        private static void GetEntitynameAndPrefixByTablename(string tablename, out string entityName, out string prefix)
        {
            prefix = string.Empty;
            tablename = tablename.Replace("Fct_", "").Replace("Rel_", "").Replace("Dim_", "");
            int i = tablename.IndexOf('_');
            if (i > -1)
            {
                prefix = tablename.Substring(0, i + 1);
                tablename = tablename.Substring(i + 1);
            }
            entityName = PluralizerHelper.ToSingular(tablename);
        }
        #endregion
    }
}
