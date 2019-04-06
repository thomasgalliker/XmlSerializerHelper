﻿using System.IO;
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

#if !NETSTANDARD1_0
        /// <inheritdoc />
        public string SerializeToXmlDocument(object value, Encoding encoding = null)
        {
            var doc = new XmlDocument();
            var nav = doc.CreateNavigator();
            using (var xmlWriter = nav.AppendChild())
            {
                this.SerializeToXml(xmlWriter, value);
            }

            encoding = encoding ?? this.Encoding;

            using (var stringWriter = new StringWriterWithEncoding(encoding))
            {
                var xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = encoding
                };

                using (var xmlTextWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    return stringWriter.GetStringBuilder().ToString();
                }
            }

            //return doc.OuterXml;
        }
#endif

        /// <inheritdoc />
        public void SerializeToXml(XmlWriter xmlWriter, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var serializer = new XmlSerializer(value.GetType());
            serializer.Serialize(xmlWriter, value);
        }

        /// <inheritdoc />
        public string SerializeToXml<T>(T value, bool preserveTypeInformation = false, Encoding encoding = null)
        {
            return this.SerializeToXml(typeof(T), value, preserveTypeInformation, encoding);
        }


        /// <inheritdoc />
        public string SerializeToXml(Type sourceType, object value, bool preserveTypeInformation = false, Encoding encoding = null)
        {
            encoding = encoding ?? this.Encoding;

            if (sourceType.GetTypeInfo().IsInterface && value != null)
            {
                sourceType = value.GetType();
            }

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

            var mainType = objectToSerialize?.GetType() ?? sourceType;
            var extraTypes = new[] { sourceType };
            var serializer = new XmlSerializer(mainType, extraTypes);

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
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (string.IsNullOrEmpty(xmlString))
            {
                throw new ArgumentException("Must not be null or empty", nameof(xmlString));
            }

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