using System;
using System.Text;
using InstagramPhotos.CodeGender.Helper;

namespace InstagramPhotos.CodeGender.Extension
{
    public static class StringExtension
    {
        public static string GetAlias(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            name = Coder.Coder.ConvertTablenameToClassname(name);

            StringBuilder alias = new StringBuilder();
            char[] chars = name.ToCharArray();
            alias.Append(chars[0]);
            for(int i=1; i<chars.Length; i++)
            {
                char c = chars[i];
                if (c >= 65 && c <= 90)
                    alias.Append(c);
            }
            return alias.ToString();
        }

        public static string IncreaseIndent(this string rawText, int tabCount)
        {
            StringBuilder text = new StringBuilder();
            foreach (string line in rawText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                text.AppendLineWithTabs(line, tabCount);
            }
            return text.ToString();
        }

        public static string ToFirstLower(this string rawText)
        {
            if (string.IsNullOrEmpty(rawText))
                return rawText;

            return string.Format("{0}{1}", rawText.Substring(0, 1).ToLower(), rawText.Substring(1));
        }

        public static string ToFirstUpper(this string rawText)
        {
            if (string.IsNullOrEmpty(rawText))
                return rawText;

            return string.Format("{0}{1}", rawText.Substring(0, 1).ToUpper(), rawText.Substring(1));
        }

        public static string ToPlural(this string rawText)
        {
            if (string.IsNullOrEmpty(rawText))
                return rawText;

            return PluralizerHelper.ToPlural(rawText);
        }

        public static string ToSingular(this string rawText)
        {
            if (string.IsNullOrEmpty(rawText))
                return rawText;

            return PluralizerHelper.ToSingular(rawText);
        }
    }
}
