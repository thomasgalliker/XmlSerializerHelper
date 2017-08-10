using System.Text;

namespace System.Xml.Serialization
{
    /// <summary>
    /// Abstraction of the XmlSerializerHelper.
    /// </summary>
    public interface IXmlSerializerHelper
    {
        /// <summary>
        /// The encoding used to serialize and deserialize xml strings.
        /// </summary>
        Encoding Encoding { get; set; }

#if NETFX
        /// <summary>
        /// Serializes XML-serializable objects to XML documents.
        /// </summary>
        string SerializeToXmlDocument(object value, Encoding encoding = null);
#endif

        /// <summary>
        /// Serializes XML-serializable objects to XML documents.
        /// </summary>
        void SerializeToXml(XmlWriter xmlWriter, object value);

        /// <summary>
        /// Serializes objects into XML strings.
        /// </summary>
        /// <param name="value">The object to be serialized.</param>
        /// <param name="preserveTypeInformation">
        /// Instructs the serializer to preserve the original type of the given value.
        /// This flag must be set to <value>true</value> when you intend to deserialize to an interface type.
        /// Default value is <value>false</value>..</param>
        /// <param name="encoding">The string encoding.</param>
        /// <returns>The serialized XML string.</returns>
        string SerializeToXml<T>(T value, bool preserveTypeInformation = false, Encoding encoding = null);

        /// <summary>
        /// Serializes objects into XML strings.
        /// </summary>
        /// <param name="sourceType">The type of the object to be serialized.</param>
        /// <param name="value">The object to be serialized.</param>
        /// <param name="preserveTypeInformation">
        /// Instructs the serializer to preserve the original type of the given value.
        /// This flag must be set to <value>true</value> when you intend to deserialize to an interface type.
        /// Default value is <value>false</value>..</param>
        /// <param name="encoding">The string encoding.</param>
        /// <returns>The serialized XML string.</returns>
        string SerializeToXml(Type sourceType, object value, bool preserveTypeInformation = false, Encoding encoding = null);

        /// <summary>
        /// Deserializes XML strings into objects of given type T.
        /// </summary>
        /// <typeparam name="T">Target type T.</typeparam>
        /// <param name="xmlString">The serialized XML string.</param>
        /// <param name="encoding">The string encoding.</param>
        /// <returns>The deserialized object of target type.</returns>
        T DeserializeFromXml<T>(string xmlString, Encoding encoding = null);

        /// <summary>
        /// Deserializes XML strings into objects of given targetType.
        /// </summary>
        /// <typeparam name="T">Target type T.</typeparam>
        /// <param name="targetType">Target type.</param>
        /// <param name="xmlString">The serialized XML string.</param>
        /// <param name="encoding">The string encoding.</param>
        /// <returns>The deserialized object of target type.</returns>
        object DeserializeFromXml(Type targetType, string xmlString, Encoding encoding = null);
    }
}
