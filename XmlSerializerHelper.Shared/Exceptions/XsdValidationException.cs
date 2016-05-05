namespace System.Xml.Serialization.Exceptions
{
    public class XsdValidationException : Exception
    {
        public XsdValidationException(string message, string sourceUri, int lineNumber, int linePosition) : base(message)
        {
            this.SourceUri = sourceUri;
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
        }

        public int LineNumber { get; private set; }

        public int LinePosition { get; private set; }

        public string SourceUri { get; private set; }
    }
}