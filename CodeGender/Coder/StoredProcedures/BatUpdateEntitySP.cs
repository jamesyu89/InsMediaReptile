using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    public class BatUpdateEntitySP : StoredProcedure
    {
        Column idColumn;
        Table table;
        List<Column> columns;

        public BatUpdateEntitySP(string name, string author,
            List<Column> columns, Column idColumn, Table table)
            : base(name, author)
        {
            this.idColumn = idColumn;
            this.table = table;
            this.columns = columns;
            this.Description = string.Format("Update entity of {0}", table.Name);
            this.Parameters = new List<StoredProcedureParameter>();

            this.Parameters.Add(new StoredProcedureParameter("models", "XML", 0));
        }



        public override string Body
        {
            get
            {
                bool first = true;
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("UPDATE D");
                //  sql.AppendLineFormatWithTabs("[{0}]", 1, table.Name);
                sql.AppendLine("SET");
                foreach (Column column in columns)
                {
                    if (column.Name == idColumn.Name)
                        continue;

                    sql.Append("\t");
                    if (!first)
                        sql.Append(",");
                    else
                        first = false;

                    sql.AppendLineFormat("D.[{0}] = S.[{0}]", column.Name);
                }
                sql.AppendLineFormat("FROM {0} D", table.Name);
                sql.AppendLine(" INNER JOIN (");

                //
                sql.AppendLine(" SELECT ");

                bool first2 = true;
                foreach (Column column in columns)
                {
                    //if (column.Name == idColumn.Name
                    //    && ((idColumn.CSTypeName == "int") || (idColumn.CSTypeName == "Guid")))
                    //    continue;

                    sql.Append("\t");
                    if (!first2)
                        sql.Append(",");
                    else
                        first2 = false;

                    sql.AppendLineFormat("T.ts.value('@{0}','{1}') as {0}", column.Name, column.NameWithSize);
                }

                sql.AppendLineFormat(@"FROM {0}.nodes('/es/e') T(ts)", "@" + Parameters[0].Name);

                sql.AppendLine(" ) S");
                sql.AppendLineFormat("ON S.[{0}]=D.[{0}]", idColumn.Name);
                // sql.AppendLineFormat("\t[{0}] = @{0}", idColumn.Name);
                return sql.ToString();
            }
        }
    }
}
