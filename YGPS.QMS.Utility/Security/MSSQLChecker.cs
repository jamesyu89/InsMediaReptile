namespace InstagramPhotos.Utility.Security
{
    public class MSSQLChecker
    {
        // Fields
        private const string StrKeyWord =
            "select|insert|delete|from|drop table|update|truncate|exec master|netlocalgroup administrators|:|net user|or|and";

        private const string StrRegex = "=|!|'";

        // Methods
        public static bool CheckKeyWord(string _sWord)
        {
            bool result = false;
            string[] patten1 =
                "select|insert|delete|from|drop table|update|truncate|exec master|netlocalgroup administrators|:|net user|or|and"
                    .Split(new[] {'|'});
            string[] patten2 = "=|!|'".Split(new[] {'|'});
            foreach (string sqlKey in patten1)
            {
                if ((_sWord.IndexOf(" " + sqlKey) >= 0) || (_sWord.IndexOf(sqlKey + " ") >= 0))
                {
                    result = true;
                    break;
                }
            }
            foreach (string sqlKey in patten2)
            {
                if (_sWord.IndexOf(sqlKey) >= 0)
                {
                    return true;
                }
            }
            return result;
        }

        public static bool CheckSQLSecurity(string SQL)
        {
            return false;
        }
    }
}