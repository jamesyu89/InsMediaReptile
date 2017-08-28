using System.Collections.Generic;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    public class GetAssistantSP : StoredProcedure
    {
        Column idColumn;
        public static Table table;
        

        public GetAssistantSP(List<Column> assistantColumn, string name, string author,
            Column idColumn, Table table)
            : base(name, author)
        {
            this.AssistantColumn = assistantColumn;
            this.idColumn = idColumn;
            GetAssistantSP.table = table;
            this.Description = string.Format("Get id of {0} list", table.Name);
        }

        public override string Body
        {
            get
            {
                return string.Format("SELECT {0} FROM {1}(NOLOCK)", idColumn.Name, table.Name);
            }
        }

        public static string SqlStr(Column Column) 
        {
            return string.Format("SELECT {0} FROM {1}(NOLOCK)", Column.Name, table.Name);
        }
    }
}
