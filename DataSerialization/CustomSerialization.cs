using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace DataSerialization
{
    public class CustomSerialization
    {
        public void ArtistsSerializationApproachOne()
        {
            JazzArtists artist = new JazzArtists() { Name = "John Mayer", Age = 28 };
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("Artists.bin", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(fs, artist);
            }
            using (FileStream fs = new FileStream("Artists.bin", FileMode.Open, FileAccess.Read))
            {
                JazzArtists ja = (JazzArtists)formatter.Deserialize(fs);
            }
        }
        public void ArtSerializationApproachTwo()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Art art = new Art();
            using (FileStream fs = new FileStream("Artists.bin", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(fs, art);
            }
            using (FileStream fs = new FileStream("Artists.bin", FileMode.Open, FileAccess.Read))
            {
                formatter.Deserialize(fs);
            }
        }
    }
    #region [ Approach 1 ]

    /// <summary>
    /// Sometimes it might be necessary for code in a class to get control during the serialization
    /// process.You might want to add checking information or encryption to data elements, or you
    /// might want to perform some custom compression of the data.There are two ways that to do this.
    /// The first way is to create our own implementation of the serialization process by making a
    /// data class implement the ISerializable interface.
    /// </summary>
    [Serializable]
    public class Artists : ISerializable
    {
        public string Name { get; set; }
        protected Artists(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
        }
        public Artists() { }
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
        }
    }
    [Serializable]
    public class JazzArtists : Artists
    {
        public int Age { get; set; }
        public JazzArtists() : base() { }
        protected JazzArtists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Age = info.GetInt32("age");
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("age", Age);
        }

    }
    #endregion

    #region [ Approach 2 ]
    [Serializable]
    public class Art
    {
        [OnSerializing]
        public void OnSerializingMethod(StreamingContext context)
        {
            Console.WriteLine("Called before the artist is serialized");
        }
        [OnSerialized()]
        public void OnSerializedMethod(StreamingContext context)
        {
            Console.WriteLine("Called after the artist is serialized");
        }
        [OnDeserializing()]
        public void OnDeserializingMethod(StreamingContext context)
        {
            Console.WriteLine("Called before the artist is deserialized");
        }
        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
            Console.WriteLine("Called after the artist is deserialized");
        }
    } 
    #endregion
}
