using System;
using System.Text;

namespace XmlSerializerHelper.Utils
{
    public static class ByteConverter
    {
        public static String Utf8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetString(characters, 0, characters.Length);
        }

        public static Byte[] StringToUtf8ByteArray(string byteString)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(byteString);
            return byteArray;
        }
    }
}