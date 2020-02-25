using System.IO;
using System.Runtime.Serialization;

namespace DataSerialization
{
    public class DataContractSerializerXML
    {
        public void SerializeWithDataContractSerializer() => new DataContractSerializerExm().SerializeObjectWithDataContractSerializer();
    }

    public class DataContractSerializerExm
    {
        public void SerializeObjectWithDataContractSerializer()
        {
            AnimalStore ads = AnimalStore.AnimalDataStore();
            DataContractSerializer formatter = new DataContractSerializer(typeof(AnimalStore), new DataContractSerializerSettings()
            {
                MaxItemsInObjectGraph = 50
            });
            using (FileStream fs = new FileStream("Animal.xml", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.WriteObject(fs, ads);
            }

        }
    }

    [DataContract]
    public class Animal
    {
        public int Id { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
    [DataContract]
    public class Dog
    {
        [DataMember]
        public int AnimalId { get; set; }
        [DataMember]
        public bool Neutered { get; set; }
    }
    [DataContract]
    public class AnimalStore
    {
        [DataMember(Name = "Dog", IsRequired = true, Order = 2)]
        public Dog Dog { get; set; }
        [DataMember(Name = "Animal", IsRequired = true, Order = 1)]
        public Animal Animal { get; set; }
        public static AnimalStore AnimalDataStore()
        {
            AnimalStore result = new AnimalStore()
            {
                Dog = new Dog() { AnimalId = 1, Neutered = false },
                Animal = new Animal() { Age = 2, Id = 2, Name = "Doggy" }
            };
            return result;
        }
    }
}
