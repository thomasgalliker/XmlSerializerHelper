using System.Xml.Serialization;

using Xamarin.Forms;

namespace XmlSample.Forms
{
    public class App : Application
    {
        public App()
        {
            XmlSerializerHelperDemo demo = new XmlSerializerHelperDemo();
            demo.Start();

            this.MainPage = new ContentPage
            {
                Content =
                    new StackLayout
                        {
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Center,
                            Children =
                                {
                                    //new Label { HorizontalTextAlignment = TextAlignment.Start, Text = string.Format("Input: {0}", inputString) },
                                    //new Label { HorizontalTextAlignment = TextAlignment.Start, Text = string.Format("SerializeToXml: {0}", serialized) },
                                    //new Label { HorizontalTextAlignment = TextAlignment.Start, Text = string.Format("DeserializeFromXml: {0}", deserialized) },
                                }
                        }
            };
        }
    }
}