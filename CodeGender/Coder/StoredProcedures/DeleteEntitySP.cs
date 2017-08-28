using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{

    public class DeleteEntitySP : StoredProcedure
    {
        Column idColumn;
        Table table;

        public DeleteEntitySP(string name, string author,
            Column idColumn, Table table)
            : base(name, author)
        {
            this.idColumn = idColumn;
            this.table = table;
            this.Description = string.Format("Delete entity of {0}", table.Name);
            this.Parameters = new List<StoredProcedureParameter>();
            this.Parameters.Add(new StoredProcedureParameter(idColumn));
        }

        public override string Body
        {
            get
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLineFormat("DELETE FROM [{0}] WHERE {1} = @{1}", table.Name, idColumn.Name);

                return sql.ToString();
            }
        }
    }

}
