using System;
using System.Collections.Generic;
using System.Text;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Classes
{
    public class EntityDtoClass : Class
    {
        List<Column> columns;
        String tbName;
        String idName;
        bool protobuf;

        public EntityDtoClass(CodeOption option)
            : base(option.EntityModelName, option.DtoNamespaceName)
        {
            this.columns = option.Columns;
            this.protobuf = option.ProtoBuf;
            this.tbName = option.FullTableName;
            this.idName = option.IdColumn.Name;

            if (this.protobuf)
            {
                this.NameSpaces.Add("ProtoBuf");
                this.Attributes.Add("ProtoContract");
            }
            else
            {
                this.Attributes.Add("Serializable");
            }
        }

        public override string Body
        {
            get
            {
                StringBuilder code = new StringBuilder();
                StringBuilder code2 = new StringBuilder();

                code.AppendLine();

                int i = 1;

                code.AppendLine("#region Members");
                code.AppendLine();

                foreach (Column column in columns)
                {
                    //code.AppendLineFormatWithTabs("{0}{3} {1}{2};", 0, column.CSTypeName, column.Name.ToFirstLower(), GetDefaultValue(column.CSTypeName), (column.NullAble && column.CSTypeName != "string") ? "?" : string.Empty);

                    code2.AppendLineWithTabs("/// <summary>", 0);
                    code2.AppendLineFormatWithTabs("/* {1}: {0}*/", 0, column.Remarks, i);
                    code2.AppendLineWithTabs("/// </summary>", 0);
                    if (protobuf)
                        code2.AppendLineFormatWithTabs("[ProtoMember({0})]", 0, i);
                    code2.AppendLineFormatWithTabs("public {0}{3} {1} {{ get;set;}}", 0, column.CSTypeName, column.Name == Name ? "_" + column.Name : column.Name, column.Name.ToFirstLower(), (column.NullAble && column.CSTypeName != "string") ? "?" : string.Empty);

                    i++;
                }

                code.AppendLine();
                code.AppendLine("#endregion");

                code.AppendLine();
                code.Append(code2.ToString());

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
