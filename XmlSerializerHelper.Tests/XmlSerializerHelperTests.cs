using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization.Tests.TestData;

using FluentAssertions;

using Xunit;

namespace System.Xml.Serialization.Tests
{
    public class XmlSerializerHelperTests
    {
        [Fact]
        public void ShouldSerializeEmptyObject()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            object obj = new object();

            // Act

            var serializedString = xmlSerializerHelper.SerializeToXml(obj);
            var deserializedObject = xmlSerializerHelper.DeserializeFromXml<object>(serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedObject.Should().NotBeNull();
        }

        [Fact]
        public void ShouldSerializeSimpleObjectWithGenericMethod()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            object obj = new SimpleSerializerClass { BoolProperty = true, StringProperty = "test" };

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml(obj);
            var deserializedObject = xmlSerializerHelper.DeserializeFromXml<SimpleSerializerClass>(serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedObject.Should().NotBeNull();
            deserializedObject.BoolProperty.Should().BeTrue();
            deserializedObject.StringProperty.Should().Be("test");
        }

        [Fact]
        public void ShouldSerializeSimpleObjectWithNonGenericMethod()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            object obj = new SimpleSerializerClass { BoolProperty = true, StringProperty = "test" };
            Type targetType = obj.GetType();

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml(obj);
            var deserializedObject = (SimpleSerializerClass)xmlSerializerHelper.DeserializeFromXml(targetType, serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedObject.Should().NotBeNull();
            deserializedObject.BoolProperty.Should().BeTrue();
            deserializedObject.StringProperty.Should().Be("test");
        }

        [Fact]
        public void ShouldSerializeConcreteList()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            List<string> list = new List<string> { "a", "b", "c" };

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml(list);
            var deserializedList = xmlSerializerHelper.DeserializeFromXml<List<string>>(serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedList.Should().NotBeNullOrEmpty();
            deserializedList.Should().HaveCount(list.Count);
            deserializedList.ElementAt(0).Should().Be(list[0]);
            deserializedList.ElementAt(1).Should().Be(list[1]);
            deserializedList.ElementAt(2).Should().Be(list[2]);
        }

        [Fact]
        public void ShouldSerializeInterfaceList()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            IList<string> list = new List<string> { "a", "b", "c" };

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml(list, preserveTypeInformation: true);
            var deserializedList = xmlSerializerHelper.DeserializeFromXml<IList<string>>(serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedList.Should().NotBeNullOrEmpty();
            deserializedList.Should().HaveCount(list.Count);
            deserializedList.ElementAt(0).Should().Be(list[0]);
            deserializedList.ElementAt(1).Should().Be(list[1]);
            deserializedList.ElementAt(2).Should().Be(list[2]);
        }

        [Fact]
        public void ShouldDeserializeListFromXmlFile()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            var restaurantsXml = ResourceLoader.ResourceLoader.GetEmbeddedResourceString(this.GetType().Assembly, ".SerializedData.xml");
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var listOfRestaurants = xmlSerializerHelper.DeserializeFromXml<List<Restaurant>>(restaurantsXml);
            stopwatch.Stop();

            // Assert
            listOfRestaurants.Should().HaveCount(4891);
            stopwatch.Elapsed.TotalMilliseconds.Should().BeLessOrEqualTo(1500);
        }

        [Fact]
        public void ShouldDeserializeXmlWithEncoding()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            string serializedString = @"<?xml version=""1.0"" encoding=""iso-8859-1"" ?><SimpleSerializerClass><StringProperty>6.00% p.a. Multi Barrier Reverse Convertible on EURO STOXX 50® Index, S&amp;P 500®, Swiss Market Index®</StringProperty></SimpleSerializerClass>";
            var encoding = Encoding.GetEncoding("ISO-8859-1");

            // Act
            var deserializedObject = xmlSerializerHelper.DeserializeFromXml<SimpleSerializerClass>(serializedString, encoding);

            // Assert
            deserializedObject.Should().NotBeNull();
            deserializedObject.StringProperty.Should().NotContain("Â");
            xmlSerializerHelper.Encoding.Should().Be(Encoding.UTF8);
        }

        [Fact]
        public void ShouldDeserializeXmlWithEncodingMismatch()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            xmlSerializerHelper.Encoding = Encoding.UTF8;
            string serializedString = @"<?xml version=""1.0"" encoding=""iso-8859-1"" ?><SimpleSerializerClass><StringProperty>6.00% p.a. Multi Barrier Reverse Convertible on EURO STOXX 50® Index, S&amp;P 500®, Swiss Market Index®</StringProperty></SimpleSerializerClass>";

            // Act
            var deserializedObject = xmlSerializerHelper.DeserializeFromXml<SimpleSerializerClass>(serializedString);

            // Assert
            deserializedObject.Should().NotBeNull();
            deserializedObject.StringProperty.Should().Contain("Â");
        }
    }
}