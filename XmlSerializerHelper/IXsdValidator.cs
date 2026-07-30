namespace System.Xml.Serialization
{
    public interface IXsdValidator
    {
        /// <summary>
        /// Validates the given <paramref name="xmlContent"/> against the given XSD schema <paramref name="xsdContent"/>.
        /// </summary>
        /// <param name="xmlContent">The XML content as string.</param>
        /// <param name="xsdContent">The XSD schema as string.</param>
        /// <returns>The validation result which indicates success/failure.</returns>
        ValidationResult Validate(string xmlContent, string xsdContent);
    }
}