using System;
using System.Data;
using System.Text;

namespace InstagramPhotos.CodeGender.Coder.StoredProcedures
{
    public class StoredProcedureParameter
    {
        public StoredProcedureParameter(Column column)
        {
            this.Name = column.Name;
            this.DbType = column.DBTypeName;
            this.Size = column.Length;
            this.Comment = column.Remarks;
            this.Order = column.ColumnId;
            this.Precision = column.Precision;
            this.Scale = column.Scale;
        }

        public StoredProcedureParameter(string name, string type, int size)
        {
            this.Name = name;
            this.DbType = type;
            this.Size = size;
            this.ParameterDirection = ParameterDirection.Input;
        }

        public string Name { get; set; }

        public string DbType { get; set; }

        public int Size { get; set; }

        public string Comment{get;set;}

        public int Order { get; set; }

        public int Precision { get; set; }

        public int Scale { get; set; }

        public string SizeText { get; set; }

        public ParameterDirection ParameterDirection { get; set; }

        public string Sql
        {
            get
            {
                StringBuilder sql = new StringBuilder();
                string dir = string.Empty;
                if (ParameterDirection == ParameterDirection.Output)
                    dir = " OUT";
                string sizeText = string.Empty;
                if (DbType.IndexOf("char", StringComparison.InvariantCultureIgnoreCase) > -1)
                    sizeText = string.Format("({0})", Size);
                if (DbType == "decimal")
                {
                    sizeText = string.Format("({0},{1})", this.Precision, this.Scale);
                }

                SizeText = sizeText;
                sql.AppendFormat("@{0}{1}--{2}.{3}", Name.PadRight(28, ' '), (DbType + sizeText + dir).PadRight(33, ' '), Order, Comment);
                return sql.ToString();
            }
        }
    }
}
