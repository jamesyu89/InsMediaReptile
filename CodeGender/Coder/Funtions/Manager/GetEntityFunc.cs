using System.Collections.Generic;
using InstagramPhotos.CodeGender.Extension;

namespace InstagramPhotos.CodeGender.Coder.Funtions.Manager
{

    public class GetEntityFunc : Function
    {
        FunctionOption _option = null;

        public GetEntityFunc(FunctionOption option)
            : base(string.Format("Get{0}", option.EntityClass ),option .ReturnType, null)
        {
            _option = option;         
            Parameters = new List<FunctionParameter>();
            Parameters.Add(new FunctionParameter(_option.IdColumn.Name.ToFirstLower(), string.Format("{0}", _option.IdColumn.CSTypeName)));
            Parameters.Add(new FunctionParameter("cacheable", "bool"));
        }

        public override string Body
        {
            get
            {
                string template = @"
[EntityClass] [ParamEntity] = null;
if (!cacheable ||
    ![EntityCache].TryGetValue([ParamEntityId], out [ParamEntity]))
{
    [IdType][] [ParamEntityIds] = new [IdType][] { [ParamEntityId] };
    List<[EntityClass]> [ParamEntities] = [GetEntitiesFunc]([ParamEntityIds], cacheable);
    if ([ParamEntities] != null && [ParamEntities].Count == 1)
    {
        [ParamEntity] = [ParamEntities][0];
    }
}
return [ParamEntity];
";

                template = template.Replace("[ParamEntityIds]",_option .IdColumn.Name.ToFirstLower().ToPlural());
                template = template.Replace("[ParamEntities]",_option.EntityClass.ToFirstLower().ToPlural());
                template = template.Replace("[ParamEntityId]", _option .IdColumn.Name.ToFirstLower());
                template = template.Replace("[ParamEntity]", _option.EntityClass.ToFirstLower());
                template = template.Replace("[IdType]", _option.IdColumn.CSTypeName);
                template = template.Replace("[EntityClass]",_option.EntityClass  );
                template = template.Replace("[EntityCache]",_option.EntityCacheName );
                template = template.Replace("[GetEntitiesFunc]",_option.FunctionName);

                return template;
            }
        }
    }

}
