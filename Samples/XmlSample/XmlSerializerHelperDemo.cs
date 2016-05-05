using System;
using System.Xml.Serialization;

namespace XmlSample
{
    public class XmlSerializerHelperDemo
    {
        public void Start()
        {
            string inputString = "This is a test string";

            try
            {
                XsdValidator x = new XsdValidator();
                var result = x.Validate("", "");
            }
            catch (Exception ex)
            {
                
            }
           
            var serialized = XmlSerializerHelper.Current.SerializeToXml(inputString);
            var deserialized = XmlSerializerHelper.Current.DeserializeFromXml<string>(serialized);


            var xmlSerializerHelper = new XmlSerializerHelper();
            serialized = xmlSerializerHelper.SerializeToXml(inputString);
            deserialized = xmlSerializerHelper.DeserializeFromXml<string>(serialized);

            //IXsdValidator x;
        }
    }
}