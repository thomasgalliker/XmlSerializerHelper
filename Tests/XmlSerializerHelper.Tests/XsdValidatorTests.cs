using System.Linq;

using FluentAssertions;

using XmlSample.TestData;

using Xunit;

namespace System.Xml.Serialization.Tests
{
    public class XsdValidatorTests
    {
        [Fact]
        public void ShouldValidateXsdSchemaSuccessfully()
        {
            // Arrange
            string xmlContent = XmlTestData.GetValidXmlContent();
            string xsdContent = XmlTestData.GetXsdMarkup();

            IXsdValidator xsdValidator = new XsdValidator();

            // Act
            var validationResult = xsdValidator.Validate(xmlContent, xsdContent);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ShouldFailToValidateXsdSchema()
        {
            // Arrange
            string xmlContent = XmlTestData.GetInvalidXmlContent();
            string xsdContent = XmlTestData.GetXsdMarkup();

            IXsdValidator xsdValidator = new XsdValidator();

            // Act
            var validationResult = xsdValidator.Validate(xmlContent, xsdContent);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
            validationResult.Errors.ElementAt(0).Message.Should().Be("The element 'Root' has invalid child element 'Child3'. List of possible elements expected: 'Child2'.");
        }

        [Fact]
        public void ShouldAccessStaticInstance()
        {
            // Act
            IXsdValidator xsdValidator = XsdValidator.Current;

            // Assert
            xsdValidator.Should().NotBeNull();
        }
    }
}
