using System;
using System.Linq;
using System.Xml.Serialization;
using XmlSample.TestData;

namespace XmlConsoleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            IXsdValidator xsdValidator = new XsdValidator();
            var xsdContent = XmlTestData.GetXsdMarkup();

            {
                Console.WriteLine("// Demo: Validate a valid XML file against an XSD schema");
                string validXmlContent = XmlTestData.GetValidXmlContent();
                var validationResult = xsdValidator.Validate(validXmlContent, xsdContent);
                Console.WriteLine($"validationResult.IsValid: {validationResult.IsValid}");
                Console.WriteLine($"validationResult.Errors ({validationResult.Errors.Count()}):\n{FormatValidationErrors(validationResult)}");
                Console.WriteLine();
            }

            {
                Console.WriteLine("// Demo: Validate an invalid XML file against an XSD schema");
                string invalidXmlContent = XmlTestData.GetInvalidXmlContent();
                var validationResult = xsdValidator.Validate(invalidXmlContent, xsdContent);
                Console.WriteLine($"validationResult.IsValid: {validationResult.IsValid}");
                Console.WriteLine($"validationResult.Errors ({validationResult.Errors.Count()}):\n{FormatValidationErrors(validationResult)}");
                Console.WriteLine();
            }

            Console.WriteLine("// Demo: Serialize/deserialize string to XML");
            const string inputString = "This is a test string";
            Console.WriteLine($"inputString:\n{inputString}");
            Console.WriteLine();

            var serialized = XmlSerializerHelper.Current.SerializeToXml(inputString);
            Console.WriteLine($"SerializeToXml:\n{serialized}");
            Console.WriteLine();

            var deserialized = XmlSerializerHelper.Current.DeserializeFromXml<string>(serialized);
            Console.WriteLine($"DeserializeFromXml:\n{deserialized}");
            Console.WriteLine();

            Console.ReadKey();
        }

        private static string FormatValidationErrors(ValidationResult validationResult)
        {
            return $"{string.Join("\n", validationResult.Errors.Select(e => $"Line {e.LineNumber}, Position {e.LinePosition}: {e.Message}"))}";
        }
    }
}