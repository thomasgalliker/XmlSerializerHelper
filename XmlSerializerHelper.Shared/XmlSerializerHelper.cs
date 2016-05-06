using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization.Utils;

namespace System.Xml.Serialization
{
    public class XmlSerializerHelper : IXmlSerializerHelper
    {
        static readonly Lazy<IXmlSerializerHelper> Implementation = new Lazy<IXmlSerializerHelper>(CreateXmlSerializerHelper, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static IXmlSerializerHelper Current
        {
            get
            {
                return Implementation.Value;
            }
        }

        static IXmlSerializerHelper CreateXmlSerializerHelper()
        {
            return new XmlSerializerHelper();
        }

        public XmlSerializerHelper()
        {
            this.Encoding = Encoding.UTF8;
        }

        /// <inheritdoc />
        public Encoding Encoding { get; set; }

        /// <inheritdoc />
        public string SerializeToXml(object value, bool preserveTypeInformation = false, Encoding encoding = null)
        {
            //Guard.ArgumentNotNull(() => value);

            var sourceType = value.GetType();

            encoding = encoding ?? this.Encoding;

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
                using (var streamWriter = new StreamWriter(memoryStream, encoding))
                {
                    serializer.Serialize(streamWriter, objectToSerialize);
                    return ByteConverter.GetStringFromByteArray(encoding, ((MemoryStream)streamWriter.BaseStream).ToArray());
                } 
            }
        }

        /// <inheritdoc />
        public object DeserializeFromXml(Type targetType, string xmlString, Encoding encoding = null)
        {
            //Guard.ArgumentNotNullOrEmpty(() => xmlString);

            encoding = encoding ?? this.Encoding;

            if (!ValueToTypeMapping.CheckIfStringContainsTypeInformation(xmlString))
            {
                // If type information was not preserved, the targetType must not be an interface
                //Guard.ArgumentMustNotBeInterface(targetType);

                var serializer = new XmlSerializer(targetType);
                using (var memoryStream = new MemoryStream(ByteConverter.GetByteArrayFromString(encoding, xmlString)))
                {
                    var deserialized = serializer.Deserialize(memoryStream);
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

            using (var memoryStream = new MemoryStream(ByteConverter.GetByteArrayFromString(encoding, xmlString)))
            {
                deserializedObject = (ValueToTypeMapping)serializerBefore.Deserialize(memoryStream);
            }

            // If the target type is an interface, we need to deserialize again with more type information
            if (isTargetTypeAnInterface)
            {
                Type serializedType = Type.GetType(deserializedObject.TypeName);
                var serializerAfter = new XmlSerializer(typeof(ValueToTypeMapping), new[] { serializedType });
                using (var memoryStream = new MemoryStream(ByteConverter.GetByteArrayFromString(encoding, xmlString)))
                {
                    deserializedObject = (ValueToTypeMapping)serializerAfter.Deserialize(memoryStream);
                }

                return Convert.ChangeType(deserializedObject.Value, serializedType);
            }

            return deserializedObject.Value;
        }

        /// <inheritdoc />
        public T DeserializeFromXml<T>(string xmlString, Encoding encoding = null)
        {
            Type targetType = typeof(T);
            return (T)this.DeserializeFromXml(targetType, xmlString, encoding);
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