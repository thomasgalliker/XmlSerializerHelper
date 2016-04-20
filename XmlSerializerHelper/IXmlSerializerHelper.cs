namespace System.Xml.Serialization
{
    /// <summary>
    /// Abstraction of the XmlSerializerHelper.
    /// </summary>
    public interface IXmlSerializerHelper
    {
        /// <summary>
        /// Serializes objects into XML strings.
        /// </summary>
        /// <param name="value">The object to be serialized.</param>
        /// <param name="preserveTypeInformation">
        /// Instructs the serializer to preserve the original type of the given value.
        /// This flag must be set to <value>true</value> when you intend to deserialize to an interface type.
        /// Default value is <value>false</value>..</param>
        /// <returns>The serialized XML string.</returns>
        string SerializeToXml(object value, bool preserveTypeInformation = false);

        /// <summary>
        /// Deserializes XML strings into objects of given type T.
        /// </summary>
        /// <typeparam name="T">Target type T.</typeparam>
        /// <param name="xmlString">The serialized XML string.</param>
        /// <returns>The deserialized object of target type.</returns>
        T DeserializeFromXml<T>(string xmlString);

        /// <summary>
        /// Deserializes XML strings into objects of given targetType.
        /// </summary>
        /// <typeparam name="T">Target type T.</typeparam>
        /// <param name="targetType">Target type.</param>
        /// <param name="xmlString">The serialized XML string.</param>
        /// <returns>The deserialized object of target type.</returns>
        object DeserializeFromXml(Type targetType, string xmlString);
    }
}
