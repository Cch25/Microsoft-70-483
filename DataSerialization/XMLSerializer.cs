using System;
using System.IO;
using System.Xml.Serialization;

namespace DataSerialization
{
    public class SerializeXML
    {
        public void XMLSerialize()=>new XMLSerialize().SerializeXML();
    }

    public class XMLSerialize
    {
        public void SerializeXML()
        {
            MusicDataStore mds = MusicDataStore.TestData();
            XmlSerializer formatter = new XmlSerializer(typeof(MusicDataStore));
            using (FileStream fs = new FileStream("MusicTracks.xml", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(fs, mds);
            }
            using (FileStream fs = new FileStream("MusicTracks.xml", FileMode.Open, FileAccess.Read))
            {
                formatter.Deserialize(fs);
            }
        }
    }
}
