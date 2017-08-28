using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Classes
{
    public class TinyEntityClass : Class
    {
        List<Column> columns;

        public TinyEntityClass(CodeOption option)
            : base(option.ClassName, option.ModelNamespaceName)
        {
            this.columns = option.Columns;
        }

        public override string Body
        {
            get
            {
                StringBuilder code = new StringBuilder();
                code.AppendLine();
                code.AppendLineWithTabs("/// <summary>", 0);
                code.AppendLineFormatWithTabs("/// {0} OR映射构造函数", 0, Name);
                code.AppendLineWithTabs("/// </summary>", 0);
                code.AppendLineFormatWithTabs("public {0}(System.Data.IDataReader dr)", 0, Name);
                code.AppendLineWithTabs("{", 0);
                foreach (Column column in columns)
                {
                    string convert = string.Format(column.ConvertFormat,
                        string.Format("dr[\"{0}\"]", column.Name));
                    if (column.NullAble)
                    {
                        code.AppendLineFormatWithTabs("if(!Convert.IsDBNull(dr[\"{0}\"]))", 1, column.Name);
                        code.AppendLineFormatWithTabs("this.{0} = {1};", 2, column.Name, convert);
                    }
                    else
                    {
                        code.AppendLineFormatWithTabs("this.{0} = {1};", 1, column.Name, convert);
                    }
                }
                code.AppendLineWithTabs("}", 0);
                code.AppendLine();


                foreach (Column column in columns)
                {
                    code.AppendLine();
                    //code.AppendLineFormatWithTabs("{0} {1};", 0, column.CSTypeName, column.Name.ToFirstLower());
                    code.AppendLineWithTabs("/// <summary>", 0);
                    code.AppendLineFormatWithTabs("/// {0}", 0, column.Remarks);
                    code.AppendLineWithTabs("/// </summary>", 0);
                    code.AppendLineFormatWithTabs("public {0} {1} {{ get; set; }}", 0, column.CSTypeName, column.Name);
                }

                return code.ToString();
            }
        }
    }
}
