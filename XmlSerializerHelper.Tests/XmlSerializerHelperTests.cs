using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using FluentAssertions;

using XmlSample;
using XmlSample.TestData;

using Xunit;

namespace System.Xml.Serialization.Tests
{
    public class XmlSerializerHelperTests
    {
        [Fact]
        public void ShouldThrowArgumentNullExceptionIfObjectIsNull()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();

            // Act

            var serializedObject = xmlSerializerHelper.SerializeToXml<object>(null);
            var deserializedObject = xmlSerializerHelper.DeserializeFromXml<object>(serializedObject);

            // Assert
            serializedObject.Should().NotBeNullOrEmpty();
            deserializedObject.Should().BeNull();
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionIfSourceTypeIsNull()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();

            // Act

            Action action = () => xmlSerializerHelper.DeserializeFromXml(null, string.Empty);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ShouldThrowArgumentExceptionIfXmlStringIsNullOrEmpty()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();

            // Act

            Action action = () => xmlSerializerHelper.DeserializeFromXml(typeof(string), string.Empty);

            // Assert
            action.ShouldThrow<ArgumentException>();
        }

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
            object inputObject = new SimpleSerializerClass { BoolProperty = true, StringProperty = "test" };

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml(inputObject);
            var deserializedObject = xmlSerializerHelper.DeserializeFromXml<SimpleSerializerClass>(serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedObject.Should().NotBeNull();
            inputObject.ShouldBeEquivalentTo(deserializedObject);
        }

        [Fact]
        public void ShouldSerializeSimpleObjectWithNonGenericMethod()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            object inputObject = new SimpleSerializerClass { BoolProperty = true, StringProperty = "test" };
            Type targetType = inputObject.GetType();

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml(inputObject);
            var deserializedObject = (SimpleSerializerClass)xmlSerializerHelper.DeserializeFromXml(targetType, serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedObject.Should().NotBeNull();
            inputObject.ShouldBeEquivalentTo(deserializedObject);
        }

        [Fact]
        public void ShouldSerializeConcreteList()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            List<string> inputList = new List<string> { "a", "b", "c" };

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml(inputList);
            var deserializedList = xmlSerializerHelper.DeserializeFromXml<List<string>>(serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedList.Should().NotBeNullOrEmpty();
            deserializedList.Should().HaveCount(inputList.Count);
            inputList.Should().ContainInOrder(deserializedList);
        }

        [Fact]
        public void ShouldSerializeInterfaceList()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            IList<string> inputList = new List<string> { "a", "b", "c" };

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml(inputList, preserveTypeInformation: true);
            var deserializedList = xmlSerializerHelper.DeserializeFromXml<IList<string>>(serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserializedList.Should().NotBeNullOrEmpty();
            deserializedList.Should().HaveCount(inputList.Count);
            inputList.Should().ContainInOrder(deserializedList);
        }

        [Fact]
        public void ShouldSerializeNullableValue()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();

            // Act
            var serializedString = xmlSerializerHelper.SerializeToXml<int?>(null, preserveTypeInformation: true);
            var deserialized = xmlSerializerHelper.DeserializeFromXml<int?>(serializedString);

            // Assert
            serializedString.Should().NotBeNullOrEmpty();
            deserialized.Should().Be(null);
        }

        [Fact]
        public void ShouldDeserializeListFromXmlFile()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            var restaurantsXml = ResourceLoader.Current.GetEmbeddedResourceString(typeof(Restaurant).Assembly, ".SerializedData.xml");
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
            deserializedObject.StringProperty.Should().NotContain("Â", "This character is only contained if the wrong encoding is used.");
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

        [Fact]
        public void ShouldSerializeToXmlDocument()
        {
            // Arrange
            IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
            var students = new Students
            {
                Student = new[]
                {
                    new StudentsStudent
                        {
                            RollNo = 1,
                            Name = "Thomas",
                            Address = "6330 Cham"
                        }
                }
            };

            // Act
            var serializedStudents = xmlSerializerHelper.SerializeToXmlDocument(students);
            var deserializedStudents = xmlSerializerHelper.DeserializeFromXml<Students>(serializedStudents);

            // Assert
            serializedStudents.Should().NotBeNullOrEmpty();
            deserializedStudents.Should().NotBeNull();
            deserializedStudents.Student.Should().HaveCount(students.Student.Length);
            students.ShouldBeEquivalentTo(deserializedStudents, config => config.IncludingAllRuntimeProperties());
        }
    }
}