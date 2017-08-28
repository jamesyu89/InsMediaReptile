namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    public class GetIdsSP : StoredProcedure
    {
        Column idColumn;
        Table table;

        public GetIdsSP(string name, string author,
            Column idColumn, Table table)
            : base(name, author)
        {
            this.idColumn = idColumn;
            this.table = table;
            this.Description = string.Format("Get id of {0} list", table.Name);
        }

        public override string Body
        {
            get {
                return string.Format("SELECT {0} FROM {1}(NOLOCK)", idColumn.Name, table.Name);
            }
        }
    }
}
