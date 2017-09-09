using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Elvencurse2.Engine.Utilities
{
    internal static class Extensions
    {
        public static T ParseXML<T>(this string @this) where T : class
        {
            var reader = XmlReader.Create(@this.Trim().ToStream(),
                new XmlReaderSettings()
                {
                    ConformanceLevel = ConformanceLevel.Document
                });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
