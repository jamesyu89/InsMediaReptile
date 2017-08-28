using System.Collections.Generic;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.Manager
{
    public class DeleteEntityFunc : Function
    {
        string entityClass;
        string deleteEntityFuncName;
        string entityCacheName;
        string dalClass;
        Column idColumn;
        bool withTran = false;

        FunctionOption Option=null;

        public DeleteEntityFunc(FunctionOption option)
            : base(
                string.Format("Delete{0}", option.EntityClass)
                , option.ReturnType
                , null)
        {
            this.Option = option;

            Parameters = new List<FunctionParameter>();

            Parameters.Add(new FunctionParameter(option.IdColumn.Name.ToFirstLower(), option.IdColumn.CSTypeName));

            if (option.WithTran)
            {
                Parameters.Add(new FunctionParameter("tran", "TransactionManager"));
            }
        }
        

        public override string Body
        {

            get
            {
                if (Option.WithTran)
                {
                    return string.Format(templateWithTran, Option.DalClass, Option.FunctionName, Option.IdColumn.Name.ToFirstLower(), Option.EntityCacheName, Option.IdColumn.Name.ToFirstLower(), Option.EntityClass, Option.IdColumn.Name.ToFirstLower());
                }
                else
                {
                    return string.Format(template, Option.DalClass, Option.FunctionName, Option.IdColumn.Name.ToFirstLower(), Option.EntityCacheName, Option.IdColumn.Name.ToFirstLower(), Option.EntityClass, Option.IdColumn.Name.ToFirstLower());
               
                }
            }

        }

        private string template = @"
try
    {{
        DataAccess.{0}.Instance.{1}({2});
        {3}.Remove({4});
        return true;
    }}
    catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""删除{5}失败，Code:"" + {4}, e);
        return false;
    }}
";
        private string templateWithTran = @"
try
    {{
        DataAccess.{0}.Instance.{1}({2},tran.transaction);
        {3}.Remove({4});
        return true;
    }}
    catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""删除{5}失败，Code:"" + {4}, e);
        return false;
    }}
";
    }
}

