namespace System.Xml.Serialization
{
    public interface IXsdValidator
    {
        /// <summary>
        /// Validates the given <param name="xmlContent">xmlContent</param> against the given XSD schema <param name="xsdContent">xsdContent</param>.
        /// </summary>
        /// <param name="xmlContent">The XML content as string.</param>
        /// <param name="xsdContent">The XSD schema as string.</param>
        /// <returns>The validation result which indicates success/failure.</returns>
        ValidationResult Validate(string xmlContent, string xsdContent);
    }
}