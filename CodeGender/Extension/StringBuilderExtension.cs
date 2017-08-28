using System.Text;

namespace InstagramPhotos.CodeGender.Extension
{
    public static class StringBuilderExtension
    {
        public static void AppendWithTabs(this StringBuilder builder, string value, int tabCount)
        {
            for (int i = 0; i < tabCount; i++)
            {
                builder.Append("\t");
            }
            builder.Append(value);
        }

        public static void AppendLineWithTabs(this StringBuilder builder, string value, int tabCount)
        {
            for (int i = 0; i < tabCount; i++)
            {
                builder.Append("\t");
            }
            builder.AppendLine(value);
        }

        public static void AppendFormatWithTabs(this StringBuilder builder, string format
            , int tabCount, params object[] args)
        {
            for (int i = 0; i < tabCount; i++)
            {
                builder.Append("\t");
            }
            builder.AppendFormat(format, args);
        }

        public static void AppendLineFormat(this StringBuilder builder, string format,
            params object[] args)
        {
            builder.AppendLineFormatWithTabs(format, 0, args);
        }

        public static void AppendLineFormatWithTabs(this StringBuilder builder, string format
            , int tabCount, params object[] args)
        {
            builder.AppendFormatWithTabs(format, tabCount, args);
            builder.AppendLine();
        }
    }
}
