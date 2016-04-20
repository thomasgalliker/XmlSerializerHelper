namespace System.Xml.Serialization.Extensions
{
    public static class ObjectExtensions
    {
        public static string SerializeToXml(this object value, bool preserveTypeInformation = false)
        {
            return XmlSerializerHelperStatic.Current.SerializeToXml(value, preserveTypeInformation);
        }

        public static T DeserializeFromXml<T>(this string xmlString)
        {
            return XmlSerializerHelperStatic.Current.DeserializeFromXml<T>(xmlString);
        }

        public static object DeserializeFromXml(this string xmlString, Type targetType)
        {
            return XmlSerializerHelperStatic.Current.DeserializeFromXml(targetType, xmlString);
        }
    }
}
