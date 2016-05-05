using System.Xml.Serialization;

using Microsoft.Phone.Controls;

namespace XmlSample.WindowsPhone8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            this.InitializeComponent();

            XmlSerializerHelperDemo demo = new XmlSerializerHelperDemo();
            demo.Start();

            string inputString = "This is a test string";

            XsdValidator x = new XsdValidator();
            //var result = x.Validate("", "");
            var serialized = XmlSerializerHelper.Current.SerializeToXml(inputString);
            var deserialized = XmlSerializerHelper.Current.DeserializeFromXml<string>(serialized);

        }
    }
}