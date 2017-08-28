using System.Reflection;

namespace InstagramPhotos.Utility.FastReflection
{
    public class FieldAccessorCache : FastReflectionCache<FieldInfo, IFieldAccessor>
    {
        protected override IFieldAccessor Create(FieldInfo key)
        {
            return FastReflectionFactories.FieldAccessorFactory.Create(key);
        }
    }
}
