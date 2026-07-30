namespace System.Xml.Serialization.Tests.TestData
{
    internal static class EmbeddedResourceLoader
    {
        internal static string GetString(string resourceName)
        {
            var assembly = typeof(EmbeddedResourceLoader).Assembly;
            var manifestResourceName = assembly.GetManifestResourceNames()
                .Single(n => n.EndsWith(resourceName, StringComparison.InvariantCulture));

            using (var stream = assembly.GetManifestResourceStream(manifestResourceName)!)
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
