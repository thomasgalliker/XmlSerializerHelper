using System.Text;

namespace System.Xml.Serialization.Utils
{
    internal static class ByteConverter
    {
        internal static string GetStringFromByteArray(Encoding encoding, byte[] characters)
        {
            return encoding.GetString(characters, 0, characters.Length);
        }

        internal static byte[] GetByteArrayFromString(Encoding encoding, string byteString)
        {
            var byteArray = encoding.GetBytes(byteString);
            return byteArray;
        }
    }
}