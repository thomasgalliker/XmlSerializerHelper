# XmlSerializerHelper
<img src="https://raw.githubusercontent.com/thomasgalliker/XmlSerializerHelper/master/XmlSerializerHelper.png" width="100" height="100" alt="XmlSerializerHelper" align="right">
XmlSerializerHelper serializes and deserializes any .Net object from/to XML. It internally uses XmlSerializer which is part of the System.Reflection namespace. On top of XmlSerializer it adds a feature to preserve original type information.

### Download and Install XmlSerializerHelper
This library is available on NuGet: https://www.nuget.org/packages/XmlSerializerHelper/
Use the following command to install XmlSerializerHelper using NuGet package manager console:

    PM> Install-Package XmlSerializerHelper

You can use this library in any .Net project which is compatible to PCL (e.g. Xamarin Android, iOS, Windows Phone, Windows Store, Universal Apps, etc.)

### API Usage
#### Serializing a .Net object to XML
The following unit test shows how a simple .Net object of tpe SimpleSerializerClass is serialized to XML
```
// Arrange
IXmlSerializerHelper xmlSerializerHelper = new XmlSerializerHelper();
var obj = new SimpleSerializerClass { BoolProperty = true, StringProperty = "test" };
Type targetType = obj.GetType();

// Act
string serializedString = xmlSerializerHelper.SerializeToXml(obj);

// Assert
serializedString.Should().NotBeNullOrEmpty();
```

#### Deserialize an XML string to a .Net object
The following unit test shows the above serializedString can be deserialized again:
```
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
```
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
There is a number of extension methods for type System.Object in ObjectExtensions. You can simply call SerializeToXml resp. DeserializeFromXml on any .Net object. Following unit tests gives an example:
```
// Arrange
float inputValue = 123.456f;
string expectedOuput = @"﻿<?xml version=""1.0"" encoding=""utf-8""?>"+Environment.NewLine+"<float>123.456</float>";

// Act
var serializedString = inputValue.SerializeToXml();

// Assert
serializedString.Should().Be(expectedOuput);
```

#### Dependency management
If you do not use an IoC framework, you can call ```XmlSerializerHelper.Current``` to get a singleton instance of IXmlSerializerHelper.
```
string serializedString = XmlSerializerHelper.Current.SerializeToXml(value: new object(), preserveTypeInformation: false);
```
The same applies to XsdValidator.Current. However, it is advised to register the mentioned interfaces along with their respective implementations in an IoC framework, such as Unity, Autofac, Ninject, etc...

### License
This project is Copyright &copy; 2016 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
