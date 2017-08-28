using System;
using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Classes
{
    public class QueryEntityClass : Class
    {
        List<Column> columns;
        String tbName;
        String idName;
        public QueryEntityClass(CodeOption option)
            : base(option.QueryModelName, option.QueryNamespaceName)
        {
            this.columns = option.Columns;
            this.tbName = option.FullTableName;
            this.idName = option.IdColumn.Name;
            this.ParentClass = "IQueryEntity";
            this.NameSpaces.Add("System.Collections.Generic");
            this.NameSpaces.Add("InstagramPhotos.Utility.CommonQuery");
        }

        public override string Body
        {
            get
            {
                var code = new StringBuilder();
                code.AppendLine();
                code.AppendLineFormatWithTabs("private string tablename=\"{0}\";", 0, tbName);
                code.AppendLineWithTabs("/// <summary>", 0);
                code.AppendLineWithTabs("/// 表名", 0);
                code.AppendLineWithTabs("/// </summary>", 0);
                code.AppendLineWithTabs("public override string TableName", 0);
                code.AppendLineWithTabs("{", 0);
                code.AppendLineWithTabs(" get {{return tablename; }}", 1);
                code.AppendLineFormatWithTabs(" set {{ tablename = value; }}", 1, tbName);
                code.AppendLineWithTabs("}", 0);
                code.AppendLine();
                code.AppendLineWithTabs("/// <summary>", 0);
                code.AppendLineWithTabs("/// 查询主键名", 0);
                code.AppendLineWithTabs("/// </summary>", 0);
                code.AppendLineWithTabs("protected override string IdName", 0);
                code.AppendLineWithTabs("{", 0);
                code.AppendLineFormatWithTabs(" get {{ return \"{0}\"; }}", 1, idName);
                code.AppendLineWithTabs("}", 0);
                code.AppendLine();

                code.AppendLineWithTabs("public override List<QueryParamater> QueryPars { get; set; }", 0);
                code.AppendLine();

                int i = 1;

                code.AppendLine("#region 查询条件");
                code.AppendLine();

                code.AppendLineWithTabs("/// <summary>", 0);
                code.AppendLineWithTabs("/// 通用查询条件", 0);
                code.AppendLineWithTabs("/// </summary>", 0);
                code.AppendLineWithTabs("public enum QueryEnums", 0);
                code.AppendLineWithTabs("{", 0);

                foreach (Column column in columns)
                {
                    code.AppendLineWithTabs("/// <summary>", 0);
                    code.AppendLineFormatWithTabs("/* {1}: {0}*/", 0, column.Remarks, i);
                    code.AppendLineWithTabs("/// </summary>", 0);
                    code.AppendLineFormatWithTabs("{0},", 1, column.Name == Name ? "_" + column.Name : column.Name);
                    i++;
                }

                code.AppendLineWithTabs("}", 0);
                code.AppendLine("#endregion");
                code.AppendLine();
                return code.ToString();
            }
        }
    }
}
