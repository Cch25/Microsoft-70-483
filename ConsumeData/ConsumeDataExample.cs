using ConvertSoapClient;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Xml;

namespace ConsumeData
{
    public class ConsumeDataExample
    {
        public void ConsumeJsonData() => new ConsumeDataConnections().ConsumeJSON();
        public void ConsumeXmlData() => new ConsumeDataConnections().ConsumeXML();
        public void ConsumeXmlDataInDOM() => new ConsumeDataConnections().ConsumeXMLInDOM();
        public void ConsumeSoapClient(int x, int y) => new ConsumeDataConnections().ConsumeSoapService(x,y);
    }

    public class ConsumeDataConnections
    {

        public void ConsumeJSON()
        {
            WebClient webClient = new WebClient();
            var json = webClient.DownloadString("https://jsonplaceholder.typicode.com/todos/1");
            FakeRest val = JsonConvert.DeserializeObject<FakeRest>(json);
            Console.WriteLine(val.title);
            Console.WriteLine(val.id);
            Console.WriteLine(val.completed);
        }
        public void ConsumeXML()
        {
            string XMLDocument = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                                "<MusicTrack xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"> " +
                                    "<Artist>Rob Miles</Artist> " +
                                    "<Title>My Way</Title> " +
                                    "<Length>150</Length>" +
                                "</MusicTrack>";
            using (StringReader sr = new StringReader(XMLDocument))
            {
                XmlTextReader xmlReader = new XmlTextReader(sr);
                while (xmlReader.Read())
                {
                    string description = $"Type: {xmlReader.NodeType.ToString()} Name: {xmlReader.Name} " +
                        $"Value: {xmlReader.Value}";
                    Console.WriteLine(description);
                }
            }
        }
        public void ConsumeXMLInDOM()
        {
            string XMLDocument = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                                "<MusicTrack xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"> " +
                                    "<Artist>Rob Miles</Artist> " +
                                    "<Title>My Way</Title> " +
                                    "<Length>150</Length>" +
                                "</MusicTrack>";

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XMLDocument);
            XmlElement rootElement = xml.DocumentElement;
            if (rootElement.Name != "MusicTrack")
            {
                Console.WriteLine("Not a music track");
            }
            else
            {
                string artist = rootElement["Artist"].FirstChild.Value;
                Console.WriteLine(artist);
                string title = rootElement["Title"].FirstChild.Value;
                Console.WriteLine($"{artist} {title}");
            }
        }
        public void ConsumeSoapService(int x, int y)
        {
            ConverterSoapClient client = new ConverterSoapClient(ConverterSoapClient.EndpointConfiguration.ConverterSoap);
            Console.WriteLine(client.SimpleAdditionAsync(x, y).GetAwaiter().GetResult());
        }

    }
    public class FakeRest
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
}
