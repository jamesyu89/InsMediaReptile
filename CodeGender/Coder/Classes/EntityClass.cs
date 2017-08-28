using System;
using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Classes
{
    public class EntityClass : Class
    {
        List<Column> columns;
        String tbName;
        String idName;

        public EntityClass(CodeOption option)
            : base(option.DomainModelName, option.ModelNamespaceName)
        {
            this.columns = option.Columns;
            this.tbName = option.FullTableName;
            this.idName = option.IdColumn.Name;
            this.Attributes.Add("Serializable");
        }

        public override string Body
        {
            get
            {
                StringBuilder code = new StringBuilder();
                StringBuilder code2 = new StringBuilder();

                //code.AppendLine();
                //code.AppendLineWithTabs("/// <summary>", 0);
                //code.AppendLineFormatWithTabs("/// {0} OR映射构造函数", 0, Name);
                //code.AppendLineWithTabs("/// </summary>", 0);
                //code.AppendLineFormatWithTabs("public {0}(System.Data.IDataReader dr)", 0, Name);
                //code.AppendLineWithTabs("{", 0);
                //foreach (Column column in columns)
                //{
                //    string convert = string.Format(column.ConvertFormat,
                //        string.Format("dr[\"{0}\"]", column.Name));
                //    if (column.NullAble)
                //    {
                //        code.AppendLineFormatWithTabs("if(!Convert.IsDBNull(dr[\"{0}\"]))", 1, column.Name);
                //        code.AppendLineFormatWithTabs("this.{0} = {1};", 2, column.Name == Name ? "_" + column.Name : column.Name, convert);
                //    }
                //    else
                //    {
                //        code.AppendLineFormatWithTabs("this.{0} = {1};", 1, column.Name == Name ? "_" + column.Name : column.Name, convert);
                //    }
                //}
                //code.AppendLineWithTabs("}", 0);

                code.AppendLine();
                code.AppendLineWithTabs("/// <summary>", 0);
                code.AppendLineWithTabs("/// 表名", 0);
                code.AppendLineWithTabs("/// </summary>", 0);
                code.AppendLineWithTabs("public static string TableName", 0);
                code.AppendLineWithTabs("{", 0);
                code.AppendLineFormatWithTabs(" get {{return \"{0}\"; }}", 1, tbName);
                code.AppendLineWithTabs("}", 0);
                code.AppendLine();
                code.AppendLineWithTabs("/// <summary>", 0);
                code.AppendLineWithTabs("/// 查询主键名", 0);
                code.AppendLineWithTabs("/// </summary>", 0);
                code.AppendLineWithTabs("public static string IdName", 0);
                code.AppendLineWithTabs("{", 0);
                code.AppendLineFormatWithTabs(" get {{ return \"{0}\"; }}", 1, idName);
                code.AppendLineWithTabs("}", 0);

                code.AppendLine();

                int i = 1;

                code.AppendLine("#region Members");
                code.AppendLine();

                foreach (Column column in columns)
                {
                    code2.AppendLineWithTabs("/// <summary>", 0);
                    code2.AppendLineFormatWithTabs("/* {1}: {0}*/", 0, column.Remarks, i);
                    code2.AppendLineWithTabs("/// </summary>", 0);
                    code2.AppendLineFormatWithTabs("public {0}{3} {1} {{ get;set;}}", 0, column.CSTypeName, column.Name == Name ? "_" + column.Name : column.Name, column.Name.ToFirstLower(), (column.NullAble && column.CSTypeName != "string") ? "?" : string.Empty);

                    i++;
                }

                code.AppendLine();
                code.AppendLine("#endregion");

                code.AppendLine();
                code.Append(code2.ToString());

                code.AppendLine("#region 列名");
                code.AppendLine();

                code.AppendLineWithTabs("/// <summary>", 0);
                code.AppendLineWithTabs("/// 列名枚举", 0);
                code.AppendLineWithTabs("/// </summary>", 0);
                code.AppendLineWithTabs("public enum ColumnEnum", 0);
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

        private string GetDefaultValue(string type)
        {
            switch (type)
            {
                case "string":
                    return " = string.Empty";
                case "Guid":
                    return " = Guid.Empty";
                default:
                    break;
            }

            return string.Empty;
        }
    }
}
