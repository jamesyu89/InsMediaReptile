using System.Collections.Generic;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.Manager
{
    public class UpdateEntityFunc : Function
    {
        FunctionOption _option = null;
        public UpdateEntityFunc(FunctionOption option)
            : base(string.Format("Update{0}", option.EntityClass), option.ReturnType, null)
        {
            _option = option;
            Parameters = new List<FunctionParameter>();

            if (_option.IsBat)
            {
                Parameters.Add(new FunctionParameter(_option.EntityClass.ToFirstLower() + "s", string.Format("List<{0}>", _option.EntityClass)));
            }
            else
            {
                Parameters.Add(new FunctionParameter(_option.EntityClass.ToFirstLower(), _option.EntityClass));
            }
            if (_option.WithTran)
            {
                Parameters.Add(new FunctionParameter("tran", "TransactionManager"));
            }
        }

        public override string Body
        {

            get
            {
                if (_option.IsBat)
                {
                    if (_option.WithTran)
                    {
                        return string.Format(templateBatWithTran, _option.DalClass,_option.FunctionName, _option.EntityClass.ToFirstLower() + "s", _option.EntityClass, _option.EntityCacheName, _option.IdColumn.Name);
                    }
                    else
                    {
                        return string.Format(templateBat, _option.DalClass, _option.FunctionName, _option.EntityClass.ToFirstLower() + "s", _option.EntityClass, _option.EntityCacheName, _option.IdColumn.Name);
                    }
                }
                else
                {
                    if (_option.WithTran)
                    {
                        return string.Format(templateWithTran, _option.DalClass, _option.FunctionName, _option.EntityClass.ToFirstLower(), _option.EntityCacheName, _option.IdColumn.Name, _option.EntityClass);
                    }
                    else
                    {
                        return string.Format(template, _option.DalClass, _option.FunctionName, _option.EntityClass.ToFirstLower(), _option.EntityCacheName, _option.IdColumn.Name, _option.EntityClass);
                    }
                }
            }

        }

        private string template = @"
try
    {{
        DataAccess.{0}.Instance.{1}({2});
        {3}.Remove({2}.{4});
        return true;
    }}
    catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""更新{5}失败，Code:"" + {2}.{4}, e);
        return false;
    }}
";
        private string templateWithTran = @"
try
    {{
        DataAccess.{0}.Instance.{1}({2},tran.transaction);
        {3}.Remove({2}.{4});
        return true;
    }}
    catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""更新{5}失败，Code:"" + {2}.{4}, e);
        return false;
    }}
";

        private string templateBat = @"
try
    {{
        DataAccess.{0}.Instance.{1}({2});
        {4}.Remove({2}.Select(f=>f.{5}).ToList());
        return true;
    }}
    catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""更新{3}失败，"" , e);
        return false;
    }}
";
        private string templateBatWithTran = @"
try
    {{
        DataAccess.{0}.Instance.{1}({2},tran.transaction);
        {4}.Remove({2}.Select(f=>f.{5}).ToList());
        return true;
    }}
    catch (Exception e)
    {{
        CLFramework.Utility.Log.Exception(""更新{3}失败"" , e);
        return false;
    }}
";

    }
}
