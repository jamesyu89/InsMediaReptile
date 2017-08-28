using System.Collections.Generic;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.Manager
{
    public class CreateEntityFunc : Function
    {
        string entityClass;
        string createEntitiesFuncName;
        string entityCacheName;
        string dalClass;
        Column idColumn;
        bool withTran = false;
        bool isBat = false;

        FunctionOption Option;


        public CreateEntityFunc(FunctionOption option)
            : base(
                string.Format("Create{0}", option.EntityClass)
               ,option.ReturnType
                , null)
        {
            Option = option;
            Parameters = new List<FunctionParameter>();

            if (option.IsBat)
            {
                Parameters.Add(new FunctionParameter(option.EntityClass.ToFirstLower() + "s", string.Format("List<{0}>", option.EntityClass)));
            }
            else
            {
                Parameters.Add(new FunctionParameter(option.EntityClass.ToFirstLower(), option.EntityClass));
            }

            if (option.WithTran)
            {
                Parameters.Add(new FunctionParameter("tran", "TransactionManager"));
            }
        }

        public override string Body
        {

            get
            {
                if (Option.IsBat)
                {
                    if (Option.WithTran)
                    {
                        return string.Format(templateBatWithTran, Option.DalClass, Option.FunctionName, Option.EntityClass.ToFirstLower() + "s", Option.EntityClass);
                    }
                    else
                    {

                        return string.Format(templateBat, Option.DalClass, Option.FunctionName, Option.EntityClass.ToFirstLower() + "s", Option.EntityClass);
                    }

                }
                else
                {
                    if (Option.WithTran)
                    {
                        return string.Format(templateWithTran, Option.DalClass, Option.FunctionName, Option.EntityCacheName, Option.IdColumn.Name, Option.EntityClass, Option.EntityClass.ToFirstLower());
                    }
                    else
                    {
                        return string.Format(template, Option.DalClass, Option.FunctionName, Option.EntityCacheName, Option.IdColumn.Name, Option.EntityClass, Option.EntityClass.ToFirstLower());
                    }
                }
            }

        }

        private string template = @"
try
    {{
        {5} = DataAccess.{0}.Instance.{1}({5});
        {2}.Cache({5}.{3}, {5});
        return {5};
    }}
    catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""创建{4}失败，Code:"" + {5}.{3}, e);
        return null;
    }}
";
        private string templateWithTran = @"
try
    {{
        {5} = DataAccess.{0}.Instance.{1}({5},tran.transaction);
        {2}.Cache({5}.{3}, {5});
        return {5};
    }}
catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""创建{4}失败，Code:"" + {5}.{3}, e);
        return null;
    }}
";

        private string templateBat = @"
try
    {{
        DataAccess.{0}.Instance.{1}({2});
        return true;
    }}
    catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""创建{3}失败"" , e);
        return false;
    }}
";
        private string templateBatWithTran = @"
try
    {{
        DataAccess.{0}.Instance.{1}({2},tran.transaction);
        return true;
    }}
catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""创建{3}失败，"" , e);
        return false;
    }}
";
    }
}
