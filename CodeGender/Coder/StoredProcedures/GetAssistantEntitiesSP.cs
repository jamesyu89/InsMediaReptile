using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    public class GetAssistantEntitiesSP : StoredProcedure
    {
        Column idColumn;
        Table table;
        public static Table ta;
        List<Column> columns;
        public static List<Column> col;
        public GetAssistantEntitiesSP(List<Column> assistantColumn, string name, string author,
            List<Column> columns, Column idColumn, Table table, List<string> AssistantColumnProName)
            : base(name, author)
        {
            this.idColumn = idColumn;
            this.table = table;
            GetAssistantEntitiesSP.ta = table;
            this.columns = columns;
            GetAssistantEntitiesSP.col = columns;
            this.Description = string.Format("Get entity of {0} list", table.Name);
            this.Parameters = new List<StoredProcedureParameter>();
            this.Parameters.Add(new StoredProcedureParameter("Ids", "XML", 0));
            this.AssistantColumn = assistantColumn;
            this.AssistantColumnProName = AssistantColumnProName;
        }
        //public GetAssistantEntitiesSP():base() { }
        public override string Body
        {
            get
            {
                string idsName = "Ids";
                if (HasParameter)
                {
                    idsName = Parameters[0].Name;
                }

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SET NUMERIC_ROUNDABORT OFF");
                sql.AppendLine("SELECT");
                sql.AppendLineFormatWithTabs("{1}.[{0}]", 1, idColumn.Name, table.Alias);

                foreach (Column column in columns)
                {
                    if (column.Name == idColumn.Name)
                        continue;
                    sql.AppendLineFormatWithTabs(", [{0}]", 1, column.Name);
                }

                sql.AppendLine("FROM");
                sql.AppendLineFormatWithTabs("[{0}] {1}", 1, table.Name, table.Alias);
                sql.AppendLineWithTabs("JOIN", 1);
                sql.AppendLineWithTabs("(", 1);
                sql.AppendLineFormatWithTabs("SELECT {0}I.{1}.value('@i','{2}') as {3}", 2, table.Alias, idsName, idColumn.DBTypeName == "varchar" ? idColumn.DBTypeName + "(" + idColumn.Length.ToString() + ")" : idColumn.DBTypeName, idColumn.Name);
                sql.AppendLineFormatWithTabs("FROM @{0}.nodes('/es/e') {1}I({0}) ", 2, idsName, table.Alias);
                sql.AppendLineFormatWithTabs(") x{0}I ON {0}.{1} = x{0}I.{1}", 1, table.Alias, idColumn.Name);

                return sql.ToString();
            }
        }

        public static string SqlStr(Column column) 
        {
            string idsName = "Ids";            
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SET NUMERIC_ROUNDABORT OFF");
            sql.AppendLine("SELECT");
            sql.AppendLineFormatWithTabs("{1}.[{0}]", 1, column.Name, ta.Alias);

            foreach (Column item in col)
            {
                if (item.Name == column.Name)
                    continue;
                sql.AppendLineFormatWithTabs(", [{0}]", 1, item.Name);
            }

            sql.AppendLine("FROM");
            sql.AppendLineFormatWithTabs("[{0}] {1}", 1, ta.Name, ta.Alias);
            sql.AppendLineWithTabs("JOIN", 1);
            sql.AppendLineWithTabs("(", 1);
            sql.AppendLineFormatWithTabs("SELECT {0}I.{1}.value('@i','{2}') as {3}", 2, ta.Alias, idsName, column.DBTypeName == "varchar" ? column.DBTypeName + "(" + column.Length.ToString() + ")" : column.DBTypeName, column.Name);
            sql.AppendLineFormatWithTabs("FROM @{0}.nodes('/es/e') {1}I({0}) ", 2, idsName, ta.Alias);
            sql.AppendLineFormatWithTabs(") x{0}I ON {0}.{1} = x{0}I.{1}", 1, ta.Alias, column.Name);

            return sql.ToString();
        }
    }
}
