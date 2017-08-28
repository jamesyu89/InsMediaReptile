using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Coder.Funtions;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Classes
{
    public abstract class Class
    {
        public List<string> NameSpaces { get; set; }

        public List<Function> Functions { get; set; }

        public List<string> Refrences{get;set;}
        
        public List<string> Attributes { get; set; }

        public string NameSpace { get; set; }

        public string ClassType { get; set; }

        public string ParentClass { get; set; }

        public bool HasNameSpace {
            get {
                return !string.IsNullOrEmpty(NameSpace);
            }
        }

        public Class(string name, string nameSpace)
        {
            this.Name = name;
            this.NameSpace = nameSpace;
            this.WithDefaultConstructor = true;
            this.Attributes = new List<string>();
            this.Refrences = new List<string>();

            Functions = new List<Function>();
            NameSpaces = new List<string>();
            NameSpaces.Add("System");
        }

        public Class(string name, string nameSpace, List<string> refrences, List<string> attributes)
            : this(name, nameSpace)
        {
            this.Refrences = refrences;
            this.Attributes = attributes;

            Refrences.ForEach(r => NameSpaces.Add(r));
        }

        public bool WithDefaultConstructor { get; set; }

        public string Name { get; set; }

        public abstract string Body { get; }

        public string Code {
            get {
                StringBuilder code = new StringBuilder();
                foreach (string ns in NameSpaces)
                {
                    code.AppendLineFormat("using {0};", ns);
                }
                code.AppendLine();
                if (HasNameSpace)
                {
                    code.AppendLineFormatWithTabs("namespace {0}", 0, NameSpace);
                    code.AppendLine("{");
                }
                int tabCount = HasNameSpace ? 1 : 0;

                code.AppendLineWithTabs("/// <summary>", tabCount);
                code.AppendLineFormatWithTabs("/// {0}", tabCount, Name);
                code.AppendLineWithTabs("/// </summary>", tabCount);
                foreach (string at in Attributes)
                {
                    code.AppendLineFormatWithTabs("[{0}]",tabCount, at);
                }
                code.AppendLineFormatWithTabs("public {1} class {0} {2}", tabCount, Name, ClassType,string.IsNullOrEmpty(ParentClass)?string.Empty:":"+ParentClass);
                code.AppendLineWithTabs("{", tabCount);

                if (WithDefaultConstructor)
                {
                    code.AppendLine();
                    code.AppendLineWithTabs("/// <summary>", tabCount + 1);
                    code.AppendLineFormatWithTabs("/// {0} 构造函数", tabCount + 1, Name);
                    code.AppendLineWithTabs("/// </summary>", tabCount + 1);
                    code.AppendLineFormatWithTabs("public {0}()", tabCount + 1, Name);
                    code.AppendLineWithTabs("{}", tabCount + 1);
                    code.AppendLine();
                }

                code.AppendLine(Body.IncreaseIndent(tabCount + 1));

                foreach (Function func in Functions)
                {
                    code.AppendLine(func.Code.IncreaseIndent(tabCount + 1));
                    code.AppendLine();
                }

                code.AppendLineWithTabs("}", tabCount);
                if (HasNameSpace)
                {
                    code.AppendLine("}");
                }
                return code.ToString();
            }        
        }
    }
}
