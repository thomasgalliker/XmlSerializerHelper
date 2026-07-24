# XmlSerializerHelper
[![Version](https://img.shields.io/nuget/v/XmlSerializerHelper.svg)](https://www.nuget.org/packages/XmlSerializerHelper) [![Downloads](https://img.shields.io/nuget/dt/XmlSerializerHelper.svg)](https://www.nuget.org/packages/XmlSerializerHelper) [![Buy Me a Coffee](https://img.shields.io/badge/support-buy%20me%20a%20coffee-FFDD00)](https://buymeacoffee.com/thomasgalliker)

<img src="https://raw.githubusercontent.com/thomasgalliker/XmlSerializerHelper/master/logo.png" width="100" height="100" alt="XmlSerializerHelper" align="right">

XmlSerializerHelper serializes and deserializes any .NET object from/to XML. It internally uses XmlSerializer which is part of the System.Xml.Serialization namespace. On top of XmlSerializer it adds a feature to preserve original type information.

### Download and Install XmlSerializerHelper
This library is available on NuGet: https://www.nuget.org/packages/XmlSerializerHelper
Use the following command to install XmlSerializerHelper using NuGet package manager console:

    PM> Install-Package XmlSerializerHelper

You can use this library in any .NET Standard or .NET project.

### API Usage
#### Serializing a .NET object to XML
The following unit test shows how a simple .NET object of type SimpleSerializerClass is serialized to XML:
```C#
// Arrange
IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
var obj = new SimpleSerializerClass { BoolProperty = true, StringProperty = "test" };
Type targetType = obj.GetType();

// Act
string serializedString = xmlSerializerHelper.SerializeToXml(obj);

// Assert
serializedString.Should().NotBeNullOrEmpty();
```

#### Deserialize an XML string to a .NET object
The following unit test shows the above serializedString can be deserialized again:
```C#
// Arrange
// ...

// Act
var deserializedObject = (SimpleSerializerClass)xmlSerializerHelper.DeserializeFromXml(targetType, serializedString);

// Assert
deserializedObject.Should().NotBeNull();
deserializedObject.BoolProperty.Should().BeTrue();
deserializedObject.StringProperty.Should().Be("test");
```

#### Validate the XSD schema for an XML file
XsdValidator can be used to validate XML content against a given XSD schema.
```C#
// Arrange
string xmlContent = XmlTestData.GetValidXmlContent();
string xsdContent = XmlTestData.GetXsdMarkup();

IXsdValidator xsdValidator = new XsdValidator();

// Act
var validationResult = xsdValidator.Validate(xmlContent, xsdContent);

// Assert
validationResult.IsValid.Should().BeTrue();
```

#### Using object extension methods
There is a number of extension methods for type System.Object in ObjectExtensions. You can simply call SerializeToXml resp. DeserializeFromXml on any .NET object. Following unit test gives an example:
```C#
// Arrange
float inputValue = 123.456f;

// Act
var serializedString = inputValue.SerializeToXml();

// Assert
serializedString.Should().NotBeNullOrEmpty();
```

#### Dependency management
If you do not use an IoC framework, you can call ```XmlSerializerHelper.Current``` to get a singleton instance of IXmlSerializerHelper.
```C#
string serializedString = XmlSerializerHelper.Current.SerializeToXml(value: new object(), preserveTypeInformation: false);
```
The same applies to XsdValidator.Current. However, it is advised to register the mentioned interfaces along with their respective implementations in an IoC framework.

### License
This project is Copyright &copy; 2026 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker).
