using System.Xml.Serialization;

namespace XmlSample
{
    public class XmlSerializerHelperDemo
    {
        public void Start()
        {
            string inputString = "This is a test string";

            ////try
            ////{
            ////    string xmlContent = XmlTestData.GetValidXmlContent();
            ////    string xsdContent = XmlTestData.GetXsdMarkup();

            ////    IXsdValidator xsdValidator = new XsdValidator();
            ////    var validationResult = xsdValidator.Validate(xmlContent, xsdContent);
            ////}
            ////catch (Exception ex)
            ////{
                
            ////}
           
            var serialized = XmlSerializerHelper.Current.SerializeToXml(inputString);
            var deserialized = XmlSerializerHelper.Current.DeserializeFromXml<string>(serialized);


            var xmlSerializerHelper = new XmlSerializerHelper();
            serialized = xmlSerializerHelper.SerializeToXml(inputString);
            deserialized = xmlSerializerHelper.DeserializeFromXml<string>(serialized);

            //IXsdValidator x;
        }
    }
}