namespace System.Xml.Serialization.Extensions
{
    public static class ObjectExtensions
    {
        public static string SerializeToXml(this object value, bool preserveTypeInformation = false)
        {
            return XmlSerializerHelper.Current.SerializeToXml(value, preserveTypeInformation);
        }

        public static T DeserializeFromXml<T>(this string xmlString)
        {
            return XmlSerializerHelper.Current.DeserializeFromXml<T>(xmlString);
        }

        public static object DeserializeFromXml(this string xmlString, Type targetType)
        {
            return XmlSerializerHelper.Current.DeserializeFromXml(targetType, xmlString);
        }
    }
}
