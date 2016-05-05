namespace System.Xml.Serialization
{
    public class XsdValidator : IXsdValidator
    {
        public ValidationResult Validate(string xmlContent, string xsdContent)
        {
            throw new InvalidOperationException("XsdValidator not supported in Portable Class Library.");
        }
    }
}