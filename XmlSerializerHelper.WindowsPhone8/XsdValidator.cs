namespace System.Xml.Serialization
{
    public class XsdValidator : IXsdValidator
    {
        public ValidationResult Validate(string xmlContent, string xsdContent)
        {
            throw new NotSupportedException("XsdValidator is currently not supported in Windows Phone 8 SL.");
        }
    }
}