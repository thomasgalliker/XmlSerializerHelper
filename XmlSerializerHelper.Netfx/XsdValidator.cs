using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization.Exceptions;

namespace System.Xml.Serialization
{
    public class XsdValidator : IXsdValidator
    {
        private static XsdValidationException ToXsdValidationException(XmlSchemaException exception)
        {
            return new XsdValidationException(exception.Message, exception.SourceUri, exception.LineNumber, exception.LinePosition);
        }

        public ValidationResult Validate(string xmlContent, string xsdContent)
        {
            // TODO GATH: Use Guards here.
            if (string.IsNullOrEmpty(xmlContent))
            {
                throw new ArgumentNullException(nameof(xmlContent));
            }

            if (string.IsNullOrEmpty(xsdContent))
            {
                throw new ArgumentNullException(nameof(xsdContent));
            }

            var validationExceptions = new List<XsdValidationException>();
            var readerSettings = GetXmlReaderSettings(
                xsdContent: xsdContent,
                validationFunction: (obj, eventArgs) => validationExceptions.Add(ToXsdValidationException(eventArgs.Exception)));

            using (var objXmlReader = XmlReader.Create(new StringReader(xmlContent), readerSettings))
            {
                try
                {
                    while (objXmlReader.Read()) { }
                }
                catch (XmlSchemaException exception)
                {
                    validationExceptions.Add(ToXsdValidationException(exception));
                }
            }

            return new ValidationResult(validationExceptions);
        }



        private static XmlReaderSettings GetXmlReaderSettings(string xsdContent, Action<object, ValidationEventArgs> validationFunction)
        {         
            var schema = XmlSchema.Read(
                reader: new StringReader(xsdContent), 
                validationEventHandler: (obj, eventArgs) => validationFunction(obj, eventArgs));

            var readerSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
            readerSettings.Schemas.Add(schema);

            return readerSettings;
        }
    }
}
