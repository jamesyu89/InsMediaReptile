using System.Xml;

namespace InstagramPhotos.Utility.Extension
{
    public static class XmlNodeExtension
    {
        public static string GetStringAttribute(this XmlNode node, string key, string defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            if (attributes != null && (attributes[key] != null
                                       && !string.IsNullOrEmpty(attributes[key].Value)))
                return attributes[key].Value;
            return defaultValue;
        }

        public static int GetIntAttribute(this XmlNode node, string key, int defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            int val = defaultValue;

            if (attributes != null && (attributes[key] != null
                                       && !string.IsNullOrEmpty(attributes[key].Value)))
            {
                int.TryParse(attributes[key].Value, out val);
            }
            return val;
        }

        public static float GetFloatAttribute(this XmlNode node, string key, float defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            float val = defaultValue;

            if (attributes != null && (attributes[key] != null
                                       && !string.IsNullOrEmpty(attributes[key].Value)))
            {
                float.TryParse(attributes[key].Value, out val);
            }
            return val;
        }

        public static bool GetBoolAttribute(this XmlNode node, string key, bool defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            bool val = defaultValue;

            if (attributes != null && (attributes[key] != null
                                       && !string.IsNullOrEmpty(attributes[key].Value)))
            {
                bool.TryParse(attributes[key].Value, out val);
            }
            return val;
        }
    }
}