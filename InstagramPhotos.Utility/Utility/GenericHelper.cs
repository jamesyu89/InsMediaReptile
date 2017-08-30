
namespace InstagramPhotos.Utility.Utility
{
    public static class GenericHelper
    {
        public static V GenericCast<U, V>(U obj)
        {
            //return (V)Convert.ChangeType(obj, typeof(V));
            return (V)(object)obj;
        }
    }
}
