using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

using XmlSerializerHelper.Utils;

namespace XmlSerializerHelper
{
    public static class XmlSerializerHelper
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
        public static string SerializeToXml(this object value, bool preserveTypeInformation = false)
        {
            //Guard.ArgumentNotNull(() => value);

            var sourceType = value.GetType();

            object objectToSerialize;
            if (preserveTypeInformation)
            {
                objectToSerialize = new ValueToTypeMapping
                {
                    Value = value,
                    TypeName = sourceType.FullName
                };
            }
            else
            {
                objectToSerialize = value;
            }

            var serializer = new XmlSerializer(objectToSerialize.GetType(), new[] { sourceType });

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    serializer.Serialize(streamWriter, objectToSerialize);
                    return ByteConverter.Utf8ByteArrayToString(((MemoryStream)streamWriter.BaseStream).ToArray());
                } 
            }
        }

        /// <summary>
        /// Deserializes XML strings into objects of given type T.
        /// </summary>
        /// <typeparam name="T">Target type T.</typeparam>
        /// <param name="xmlString">The serialized XML string.</param>
        /// <returns>An object of type T.</returns>
        public static T DeserializeFromXml<T>(this string xmlString)
        {
            //Guard.ArgumentNotNullOrEmpty(() => xmlString);

            Type targetType = typeof(T);

            if (!ValueToTypeMapping.CheckIfStringContainsTypeInformation(xmlString))
            {
                // If type information was not preserved, the targetType must not be an interface
                //Guard.ArgumentMustNotBeInterface(targetType);

                var serializer = new XmlSerializer(targetType);
                using (var memoryStream = new MemoryStream(ByteConverter.StringToUtf8ByteArray(xmlString)))
                {
                    var deserialized = (T)serializer.Deserialize(memoryStream);
                    return deserialized;
                }
            }
            
            bool isTargetTypeAnInterface = targetType.GetTypeInfo().IsInterface;
            Type[] extraTypes = { };
            if (!isTargetTypeAnInterface)
            {
                extraTypes = new[] { targetType };
            }

            var serializerBefore = new XmlSerializer(typeof(ValueToTypeMapping), extraTypes);
            ValueToTypeMapping deserializedObject = null;

            using (var memoryStream = new MemoryStream(ByteConverter.StringToUtf8ByteArray(xmlString)))
            {
                deserializedObject = (ValueToTypeMapping)serializerBefore.Deserialize(memoryStream);
            }

            // If the target type is an interface, we need to deserialize again with more type information
            if (isTargetTypeAnInterface)
            {
                Type serializedType = Type.GetType(deserializedObject.TypeName);
                var serializerAfter = new XmlSerializer(typeof(ValueToTypeMapping), new[] { serializedType });
                using (var memoryStream = new MemoryStream(ByteConverter.StringToUtf8ByteArray(xmlString)))
                {
                    deserializedObject = (ValueToTypeMapping)serializerAfter.Deserialize(memoryStream);
                }

                return (T)Convert.ChangeType(deserializedObject.Value, serializedType);
            }

            return (T)deserializedObject.Value;
        }

        public class ValueToTypeMapping
        {
            private static readonly string Identifier = "ValueToTypeMapping_663FBB7F-9C0A-400C-A9C4-76ACADE8C741";

            public string Id
            {
                get
                {
                    return Identifier;
                }
                set
                {
                }
            }

            public static bool CheckIfStringContainsTypeInformation(string xmlString)
            {
                return xmlString.Contains(Identifier);
            }

            public string TypeName { get; set; }

            public object Value { get; set; }
        }
    }
}