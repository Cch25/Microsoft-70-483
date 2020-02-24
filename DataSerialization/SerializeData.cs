using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataSerialization
{
    public class SerializeData
    {
        public void BinarySerialization() => new BinarySerialization().SerializeData();
        public void BinaryDeserialization() => new BinarySerialization().DeserializeData();

    }
    [Serializable]
    public class Artist
    {
        public string Name { get; set; }
        /// <summary>
        /// Binary serialization is the only serialization technique that serializes private data members by default
        /// </summary>
        [NonSerialized]
        readonly int _tempInfo;
    }
    [Serializable]
    public class MusicTrack
    {
        public Artist Artist { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
    }
    [Serializable]
    public class MusicDataStore
    {
        public List<Artist> Artists = new List<Artist>();
        public List<MusicTrack> MusicTracks = new List<MusicTrack>();
        public static MusicDataStore TestData()
        {
            MusicDataStore result = new MusicDataStore()
            {
                Artists = new List<Artist>() { new Artist { Name = "John Mayer" }, new Artist { Name = "Olafur Arnolds" } },
                MusicTracks = new List<MusicTrack>() { new MusicTrack { Name = "JM", Title = "How to be loved" } }
            };
            return result;
        }
    }
    public class BinarySerialization
    {
        public void SerializeData()
        {
            MusicDataStore mds = MusicDataStore.TestData();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("MusicTracks.bin", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(fs, mds);
            }
        }
        public void DeserializeData()
        {
            MusicDataStore mds = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("MusicTracks.bin", FileMode.Open, FileAccess.Read))
            {
                mds = (MusicDataStore)formatter.Deserialize(fs);
            }
            Console.WriteLine(mds.Artists.FirstOrDefault()?.Name);
            Console.WriteLine(mds.MusicTracks.First()?.Title);
        }
    }
}
