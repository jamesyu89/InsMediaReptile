using System.Collections.Generic;

namespace InstagramPhotos.CodeGender.Coder.Funtions.Manager
{
    public class GetEntitySetFunc : Function
    {
        FunctionOption _option = null;
        public GetEntitySetFunc(FunctionOption option)
            : base(string.Format("Get{0}Set",option.EntityClass) ,option.ReturnType , null)
        {
            _option = option;
            Parameters = new List<FunctionParameter>();
            Parameters.Add(new FunctionParameter("pageIndex", "int"));
            Parameters.Add(new FunctionParameter("pageSize", "int"));
            Parameters.Add(new FunctionParameter("idsCacheable", "bool"));
            Parameters.Add(new FunctionParameter("entitiesCacheable", "bool"));
        }

        public override string Body
        {
            get {
                string template = @"
string key = string.Format(""[EntityClass]SetIds"");
List<[IdType]> ids = null;
if (idsCacheable)
    ids = CacheHelper.Get(key) as List<[IdType]>;

if (ids == null)
{
    ids = DataAccess.[DalClassName].Instance.[GetIdsFunc]();
    CacheHelper.Insert(key, ids, CacheHelper.SecondFactorCalculate(60));
}

return [EntityCache].GetEntitySet(ids, pageIndex,
    pageSize, entitiesCacheable
    , new GetEntitiesFromDatabaseDelegate<[IdType], [EntityClass]>(DataAccess.[DalClassName].Instance.[GetEntitiesFunc]));
";
                template = template.Replace("[EntityClass]",_option.EntityClass);
                template = template.Replace("[EntityCache]",_option.EntityCacheName);
                template = template.Replace("[GetIdsFunc]",_option.FunctionName2);
                template = template.Replace("[GetEntitiesFunc]",_option.FunctionName);
                template = template.Replace("[IdType]",_option.IdColumn.CSTypeName);
                template = template.Replace("[DalClassName]", _option.DalClass);
                return template;
            }
        }
    }
}
