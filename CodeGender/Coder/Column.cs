using System;
using System.Data;
using InstagramPhotos.CodeGender.Helper;

namespace InstagramPhotos.CodeGender.Coder
{
    public class Column
    {
        public Column(IDataReader dr)
        {
            this.ColumnId = Convert.ToInt32(dr["Column_id"]);
            this.Name = Convert.ToString(dr["ColumnName"]);
            this.DBTypeName = Convert.ToString(dr["Type"]);
            this.Length = Convert.ToInt32(dr["Length"]);
            this.Precision = Convert.ToInt32(dr["Precision"]);
            this.Scale = Convert.ToInt32(dr["SCALE"]);
            this.IsIdentity = Convert.ToBoolean(dr["IDENTITY"]);
            this.IsPrimaryKey = Convert.ToBoolean(dr["PrimaryKey"]);
            this.NullAble = Convert.ToBoolean(dr["NullAble"]);
            this.Remarks = Convert.ToString(dr["ColumnDesc"]);
            this.Default = Convert.ToString(dr["Default"]);
            this.CreateTime = Convert.ToDateTime(dr["Create_Date"]);
            this.ModifyTime = Convert.ToDateTime(dr["Modify_Date"]);
            //this.IsAssistant=

            string csTypeName, convertFormat;
            TypeMappings.GetTypeName(DBTypeName, out csTypeName, out convertFormat);
            this.CSTypeName = csTypeName;
            this.ConvertFormat = convertFormat;
        }

        public bool IsPrimaryKey { get; set; }
        public bool IsAssistant { get; set; }//需要生成的副键
        public bool NullAble { get; set; }
        public int ColumnId { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public bool IsIdentity { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public string DBTypeName { get; set; }
        public string CSTypeName { get; set; }
        public string ConvertFormat { get; set; }
        public string Default { get;set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }

        public string NameWithSize
        {
            get 
            {
                string sizeText = string.Empty;
                if (DBTypeName.IndexOf("char", StringComparison.InvariantCultureIgnoreCase) > -1)
                    sizeText = string.Format("({0})", Length);
                if (DBTypeName == "decimal")
                {
                    sizeText = string.Format("({0},{1})", this.Precision, this.Scale);
                }
                return DBTypeName + sizeText;
            }
        }
    }
}
