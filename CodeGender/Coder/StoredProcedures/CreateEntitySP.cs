using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    public class CreateEntitySP : StoredProcedure
    {
        Column idColumn;
        Table table;
        List<Column> columns;
        bool needAutoGuid;

        public CreateEntitySP(string name, string author,
            List<Column> columns, Column idColumn, Table table, bool needAutoGuid) : base(name, author)
        {
            this.needAutoGuid = needAutoGuid;
            this.idColumn = idColumn;
            this.table = table;
            this.columns = columns;
            this.Description = string.Format("Create entity of {0}", table.Name);
            this.Parameters = new List<StoredProcedureParameter>();
            foreach (Column column in columns)
            {
                StoredProcedureParameter param = new StoredProcedureParameter(column);
                if (column.Name == idColumn.Name && needAutoGuid)
                {
                    if (idColumn.CSTypeName == "int")
                    {
                        param.ParameterDirection = System.Data.ParameterDirection.Output;
                    }
                    else if (idColumn.CSTypeName == "Guid")
                    {
                        param.ParameterDirection = System.Data.ParameterDirection.Output;
                    }
                }
                this.Parameters.Add(param);
            }
        }

        public override string Body
        {
            get {
                bool first = true;
                StringBuilder sql = new StringBuilder();

                if (idColumn.CSTypeName == "Guid" && needAutoGuid)
                    sql.AppendLineFormat("DECLARE @RESULT TABLE ({0} UNIQUEIDENTIFIER)\n", idColumn.Name);

                sql.AppendLine("INSERT INTO");
                sql.AppendLineFormatWithTabs("[{0}]", 1, table.Name);
                sql.AppendLine("(");
                foreach (Column column in columns)
                {
                    if ((column.Name == idColumn.Name && needAutoGuid)
                        && ((idColumn.CSTypeName == "int")||(idColumn.CSTypeName == "Guid")))
                        continue;

                    sql.Append("\t");
                    if (!first)
                        sql.Append(",");
                    else
                        first = false;

                    sql.AppendLineFormat("[{0}]", column.Name);
                }
                sql.AppendLine(")");
                if (idColumn.CSTypeName == "Guid" && needAutoGuid)
                    sql.AppendLineFormat("OUTPUT INSERTED.{0} INTO @RESULT \n", idColumn.Name);
                sql.AppendLine("VALUES");
                sql.AppendLine("(");
                first = true;
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

                    sql.AppendLineFormat("@{0}", column.Name);
                }
                sql.AppendLine(")");

                if (idColumn.CSTypeName == "int")
                {
                    sql.AppendLine();
                    sql.AppendLineFormat("SELECT @{0} = SCOPE_IDENTITY()", idColumn.Name);
                }
                else if (idColumn.CSTypeName == "Guid" && needAutoGuid)
                {
                    sql.AppendLine();
                    sql.AppendLineFormat("SELECT @{0} = {0} FROM @RESULT", idColumn.Name);
                }
                return sql.ToString();
            }
        }
    }
}
