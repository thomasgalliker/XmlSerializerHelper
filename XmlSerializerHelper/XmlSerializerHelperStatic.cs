namespace System.Xml.Serialization
{
    public static class XmlSerializerHelperStatic
    {
        static readonly Lazy<IXmlSerializerHelper> Implementation = new Lazy<IXmlSerializerHelper>(CreateXmlSerializerHelper, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static IXmlSerializerHelper Current
        {
            get
            {
                return Implementation.Value;
            }
        }

        static IXmlSerializerHelper CreateXmlSerializerHelper()
        {
            return new XmlSerializerHelper();
        }
    }
}

