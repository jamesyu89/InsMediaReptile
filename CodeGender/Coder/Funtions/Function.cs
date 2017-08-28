using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions
{
    public abstract class Function
    {

        public Function(string name, string returnType, List<FunctionParameter> parameters)
        {
            this.Name = name;
            this.ReturnType = returnType;
            this.Parameters = parameters;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ReturnType { get; set; }

        public List<FunctionParameter> Parameters { get; set; }

        public bool HasParameter
        {
            get
            {
                return Parameters != null && Parameters.Count > 0;
            }
        }

        public abstract string Body { get; }

        public string Code
        {
            get {
                StringBuilder code = new StringBuilder();

                code.AppendLineWithTabs("/// <summary>", 0);
                code.AppendLineFormatWithTabs("/// {0}", 0, Description);
                code.AppendLineWithTabs("/// </summary>", 0);
                if (HasParameter)
                {
                    foreach (FunctionParameter param in Parameters)
                    {
                        code.AppendLineFormatWithTabs("/// <param name=\"{0}\">{1}</param>", 0, param.Name, param.Description);
                    }
                }
                code.AppendFormat("public {0} {1}(", ReturnType, Name);
                if (HasParameter)
                {
                    bool firstParam = true;
                    foreach (FunctionParameter param in Parameters)
                    {
                        if (!firstParam)
                            code.AppendFormat(" ,");
                        else
                            firstParam = false;
                        code.AppendFormat("{0} {1}", param.CSType, param.Name);
                    }
                }
                code.AppendLine(")");
                code.AppendLine("{");
                code.AppendLine(Body.IncreaseIndent(1));
                code.AppendLine("}");
                return code.ToString();
            }
        }
    }
}
