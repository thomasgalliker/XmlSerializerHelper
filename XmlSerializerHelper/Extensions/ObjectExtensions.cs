using System.Text;

namespace System.Xml.Serialization.Extensions
{
    public static class ObjectExtensions
    {
        public static string SerializeToXml(this object value, bool preserveTypeInformation = false, Encoding encoding = null)
        {
            return XmlSerializerHelper.Current.SerializeToXml(value, preserveTypeInformation, encoding);
        }

        public static T DeserializeFromXml<T>(this string xmlString, Encoding encoding = null)
        {
            return XmlSerializerHelper.Current.DeserializeFromXml<T>(xmlString, encoding);
        }

        public static object DeserializeFromXml(this string xmlString, Type targetType, Encoding encoding = null)
        {
            return XmlSerializerHelper.Current.DeserializeFromXml(targetType, xmlString, encoding);
        }
    }
}
