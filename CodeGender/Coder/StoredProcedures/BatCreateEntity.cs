using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    public class BatCreateEntity : StoredProcedure
    {
        Column idColumn;
        Table table;
        List<Column> columns;
        bool needAutoGuid;

        public BatCreateEntity(string name, string author,
            List<Column> columns, Column idColumn, Table table, bool needAutoGuid)
            : base(name, author)
        {
            this.idColumn = idColumn;
            this.table = table;
            this.columns = columns;
            this.needAutoGuid = needAutoGuid;
            this.Description = string.Format("Create entity of {0}", table.Name);
            this.Parameters = new List<StoredProcedureParameter>();
             this.Parameters.Add(new StoredProcedureParameter("models","XML",0));
        }

        public override string Body
        {
            get
            {
                bool first = true;
                StringBuilder sql = new StringBuilder();

                //if (idColumn.CSTypeName == "Guid" && needAutoGuid)
                //    sql.AppendLineFormat("DECLARE @RESULT TABLE ({0} UNIQUEIDENTIFIER)\n", idColumn.Name);

                sql.AppendLine("INSERT INTO");
                sql.AppendLineFormatWithTabs("[{0}]", 1, table.Name);
                sql.AppendLine("(");
                foreach (Column column in columns)
                {
                    if ((column.Name == idColumn.Name && needAutoGuid)
                        && ((idColumn.CSTypeName == "int") || (idColumn.CSTypeName == "Guid")))
                        continue;

                    sql.Append("\t");
                    if (!first)
                        sql.Append(",");
                    else
                        first = false;

                    sql.AppendLineFormat("[{0}]", column.Name);
                }
                sql.AppendLine(")");
                //if (idColumn.CSTypeName == "Guid" && needAutoGuid)
                //    sql.AppendLineFormat("OUTPUT INSERTED.{0} INTO @RESULT \n", idColumn.Name);
                //sql.AppendLine("VALUES");
                //sql.AppendLine("(");
                first = true;
                sql.AppendLine(" SELECT ");
                foreach (Column column in columns)
                {
                    if ((column.Name == idColumn.Name && needAutoGuid)
                        && ((idColumn.CSTypeName == "int") || (idColumn.CSTypeName == "Guid")))
                        continue;

                    sql.Append("\t");
                    if (!first)
                        sql.Append(",");
                    else
                        first = false;

                    sql.AppendLineFormat(formate, column.Name,column.NameWithSize);
                }
               
                sql.AppendLineFormat(@"FROM {0}.nodes('/es/e') T(ts)","@"+Parameters[0].Name);
                return sql.ToString();
            }
        }

        private string formate = "T.ts.value('@{0}','{1}') as {0}";

       
    }
}
