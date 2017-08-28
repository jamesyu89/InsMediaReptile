using System.Linq;
using System.Text;
using InstagramPhotos.CodeGender.DB;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Classes
{
    public class EnumClass : Class
    {
        public EnumClass()
            : base("CodeEnum", "")
        {
            this.WithDefaultConstructor = false;
        }

        public override string Body
        {
            get {

                var tables = DataAccess.GetTablesWithCommaRemark();
                var codes = tables.Keys.ToList().Where(t => t.Contains("_code_")).ToList();

                StringBuilder code = new StringBuilder();
                code.AppendLine();
                code.AppendLine("public enum CodeEnum");
                code.AppendLine("{");

                foreach (string c in codes)
                {
                    code.AppendLineFormatWithTabs("/// <summary> ", 1);
                    code.AppendLineFormatWithTabs("/// {0}: {1}", 1, tables[c], c);
                    code.AppendLineFormatWithTabs("/// <summary> ", 1);
                    code.AppendLineFormatWithTabs("{0}, ", 1, c.Substring(c.IndexOf("_code_") + 6));
                    

                    //var marks = DataAccess.GetCodeMarks(c);

                    //code.AppendLine();
                    //code.AppendLineWithTabs("/// <summary>", 0);
                    //code.AppendLineFormatWithTabs("/// {0}", 0, tables[c]);
                    //code.AppendLineWithTabs("/// </summary>", 0);
                    //code.AppendLineFormatWithTabs("public enum {0} : int ", 0, c.Substring(c.IndexOf("_code_") + 6));
                    //code.AppendLineWithTabs("{", 0);

                    //int i = 1;
                    //foreach (var mark in marks)
                    //{
                    //    code.AppendLineWithTabs("/// <summary>", 1);
                    //    code.AppendLineFormatWithTabs("/// {0}", 1, mark.Description);
                    //    code.AppendLineWithTabs("/// </summary>", 1);
                    //    code.AppendLineFormatWithTabs("{0} = {1}{2}", 1, mark.CodeName, mark.CodeNo, i == marks.Count ? "" : ",");

                    //    i++;
                    //}

                    //code.AppendLineWithTabs("}", 0);
                }

                code.AppendLine("}");

                /*
                /// <summary>
                /// 预警等级
                /// </summary>
                public enum AlertLevel
                {
                    /// <summary>
                    /// 预警等级
                    /// </summary>
                    Red = 1,
                    Yellow = 2,
                    Blue = 3,
                    None = 0
                }
                 * */

                return code.ToString();
            }
        }
    }
}
