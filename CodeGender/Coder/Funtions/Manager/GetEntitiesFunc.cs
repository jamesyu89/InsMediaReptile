using System.Collections.Generic;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.Manager
{
    public class GetEntitiesFunc : Function
    {

        FunctionOption _option = null;
        public GetEntitiesFunc(FunctionOption option,string FunName)
            : base(string.Format("Get{0}", FunName.ToPlural()), option.ReturnType, null)
        {
            _option = option;
            Parameters = new List<FunctionParameter>();
            Parameters.Add(new FunctionParameter(_option.IdColumn.Name.ToFirstLower().ToPlural(), string.Format("{0}[]", _option.IdColumn.CSTypeName)));
            Parameters.Add(new FunctionParameter(string.Format("{0}Cacheable", _option.EntityClass.ToFirstLower().ToPlural()), "bool"));
        }


        public override string Body
        {
            get {
                string template = @"
Dictionary<[IdType], [EntityClass]> [ParamEntities] = new Dictionary<[IdType], [EntityClass]>();
return [EntityCache].GetEntities([ParamEntityIds], ref [ParamEntities], [ParamEntities]Cacheable,
    new GetEntitiesFromDatabaseDelegate<[IdType], [EntityClass]>(DataAccess.[DalClassName].Instance.[GetEntitiesFunc]));";

                template = template.Replace("[ParamEntityIds]", _option.IdColumn.Name.ToFirstLower().ToPlural());
                template = template.Replace("[ParamEntities]",  _option.EntityClass.ToFirstLower().ToPlural());
                template = template.Replace("[IdType]",_option.IdColumn.CSTypeName);
                template = template.Replace("[EntityClass]",_option.EntityClass);
                template = template.Replace("[EntityCache]",_option.EntityCacheName);
                template = template.Replace("[GetEntitiesFunc]",_option .FunctionName);
                template = template.Replace("[DalClassName]", _option.DalClass);

                return template;
            }
        }
    }
}
