using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    public class UpdateEntitySP : StoredProcedure
    {
        Column idColumn;
        Table table;
        List<Column> columns;

        public UpdateEntitySP(string name, string author,
            List<Column> columns, Column idColumn, Table table)
            : base(name, author)
        {
            this.idColumn = idColumn;
            this.table = table;
            this.columns = columns;
            this.Description = string.Format("Update entity of {0}", table.Name);
            this.Parameters = new List<StoredProcedureParameter>();
            foreach (Column column in columns)
            {
                StoredProcedureParameter param = new StoredProcedureParameter(column);
                this.Parameters.Add(param);
            }
        }

        public override string Body
        {
            get
            {
                bool first = true;
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("UPDATE");
                sql.AppendLineFormatWithTabs("[{0}]", 1, table.Name);
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

                    sql.AppendLineFormat("[{0}] = @{0}", column.Name);
                }
                sql.AppendLine("WHERE");
                sql.AppendLineFormat("\t[{0}] = @{0}", idColumn.Name);
                return sql.ToString();
            }
        }
    }

}
