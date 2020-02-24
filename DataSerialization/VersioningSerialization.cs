using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataSerialization
{

    public class VersioningSerialization
    {
        public void Versionate()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<Versioning> versionings = new List<Versioning>()
            {
                new Versioning() { Name = "V1", Title = "T1", Artist = new Artist() { Name = "John Mayer" } },
                new Versioning() { Name = "V1.1",Title = "T1.1", Style = "Blues", Artist = new Artist() { Name = "John Mayer" } }
            };
            using (FileStream fs = new FileStream("Versioning.bin", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(fs, versionings);
            }
            using (FileStream fs = new FileStream("Versioning.bin", FileMode.Open, FileAccess.Read))
            {
                List<Versioning> mt = (List<Versioning>)formatter.Deserialize(fs);
            }
        }
    }
    /// <summary>
    /// The OnDeserializing method can be used to set values of fields that might not be present
    /// in data that is being read from a serialized document.
    /// </summary>
    [Serializable]
    public class Versioning
    {
        public string Name { get; set; }
        public Artist Artist { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// In XML to Linq we've added a new XElement programatically (Style)
        /// If we try to deserialize that new model into an old model, it will break
        /// A solution for this problem is to add an OptionalField atttribute
        /// </summary>
        [OptionalField]
        public string Style;

        [OnDeserializing()]
        public void OnDeserializingMethod(StreamingContext context)
        {
            Style = "Unknown";
        }
    }
}
