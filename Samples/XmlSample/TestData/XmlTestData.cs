using System.Xml.Linq;

namespace XmlSample.TestData
{
    internal static class XmlTestData
    {
        /// <summary>
        /// Gets sample XSD definition.
        /// Source: https://msdn.microsoft.com/en-us/library/bb387037.aspx
        /// </summary>
        internal static string GetXsdMarkup()
        {
            string xsdMarkup =
                @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                   <xsd:element name='Root'>
                    <xsd:complexType>
                     <xsd:sequence>
                      <xsd:element name='Child1' minOccurs='1' maxOccurs='1'/>
                      <xsd:element name='Child2' minOccurs='1' maxOccurs='1'/>
                     </xsd:sequence>
                    </xsd:complexType>
                   </xsd:element>
                  </xsd:schema>";

            return xsdMarkup;
        }

        internal static string GetValidXmlContent()
        {
            var xDocument = new XDocument(
                new XElement("Root",
                    new XElement("Child1", "content1"),
                    new XElement("Child2", "content1")
                )
            );

            return xDocument.ToString();
        }

        internal static string GetInvalidXmlContent()
        {
            var xDocument = new XDocument(
                new XElement("Root",
                    new XElement("Child1", "content1"),
                    new XElement("Child3", "content1")
                )
            );

            return xDocument.ToString();
        }
    }
}
