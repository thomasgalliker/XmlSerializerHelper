using System.Xml.Serialization.Extensions;

namespace System.Xml.Serialization.Tests.Extensions
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void ShouldTestSerializeToXmlExtensionMethod()
        {
            // Arrange
            float inputValue = 123.456f;
            string expectedOutput = "﻿" + @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n<float>123.456</float>";

            // Act
            var serializedString = inputValue.SerializeToXml();

            // Assert
            serializedString.Replace("\r\n", "\n").Should().Be(expectedOutput);
        }

        [Fact]
        public void ShouldTestDeserializeFromXmlExtensionMethodGeneric()
        {
            // Arrange
            string serializedString = "﻿" + @"<?xml version=""1.0"" encoding=""utf-8""?><float>123.456</float>";
            float expectedOutput = 123.456f;

            // Act
            var deserializedObject = serializedString.DeserializeFromXml<float>();

            // Assert
            deserializedObject.Should().Be(expectedOutput);
        }

        [Fact]
        public void ShouldTestDeserializeFromXmlExtensionMethodNonGeneric()
        {
            // Arrange
            string serializedString = "﻿" + @"<?xml version=""1.0"" encoding=""utf-8""?><float>123.456</float>";
            float expectedOutput = 123.456f;

            // Act
            var deserializedObject = serializedString.DeserializeFromXml(typeof(float));

            // Assert
            deserializedObject.Should().Be(expectedOutput);
        }
    }
}
