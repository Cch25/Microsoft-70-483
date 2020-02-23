using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace QueryData
{
    public class LinqToXML
    {
        public void ReadXml() => new XMLHelper().ReadXml();
        public void FilterXml() => new XMLHelper().FilterXml("Rob Miles ");
        public void CreateAddModifyXmlWithLinq() => new XMLHelper().CreateAddModifyXmlWithLinq();
    }

    public class XMLHelper
    {
        private readonly string XMLText = $"<MusicTracks> " +
                                               "<MusicTrack> " +
                                                   "<Artist>Rob Miles </Artist> " +
                                                   "<Title>My Way </Title> " +
                                                   "<Length>150</Length>" +
                                               "</MusicTrack>" +
                                               "<MusicTrack>" +
                                                   "<Artist>Immy Brown </Artist> " +
                                                   "<Title>Her Way </Title> " +
                                                   "<Length>200 </Length>" +
                                               "</MusicTrack>" +
                                           "</MusicTracks>";
        public void ReadXml()
        {
            XDocument documentParse = XDocument.Parse(XMLText);
            IEnumerable<XElement> selectedTracks =
                from track in documentParse.Descendants("MusicTrack")
                select track;

            selectedTracks.ToList().ForEach(x => Console.WriteLine(x.Value));

            documentParse.Descendants("MusicTrack")
                         .Select(x => x.Value)
                         .ToList()
                         .ForEach(x => Console.WriteLine(x));
        }
        public void FilterXml(string artist)
        {
            XDocument documentParse = XDocument.Parse(XMLText);
            var filteredDoc = from track in documentParse.Descendants("MusicTrack")
                              where (string)track.Element("Artist") == artist
                              select track;
            Console.WriteLine(filteredDoc.FirstOrDefault()?.Value);

            documentParse
                .Descendants("MusicTrack")
                .Where(x => (string)x.Element("Artist") == artist)
                .ToList()
                .ForEach(y => Console.WriteLine(y.Value));
        }
        public void CreateAddModifyXmlWithLinq()
        {
            XElement musicTracks = new XElement("MusicTracks",
                new List<XElement>
                {
                    new XElement("MusicTrack",
                        new XElement("Artist","Rob Miles "),
                        new XElement("Title","My Way ")),
                    new XElement("MusicTrack",
                        new XElement("Artist","Bear Bearrison "),
                        new XElement("Title","Me being a bear "))
                });
            //edit a node
            XDocument xDoc = XDocument.Parse(musicTracks.ToString());
            xDoc.Descendants("MusicTrack")
                .First(x => (string)x.Element("Artist") == "Rob Miles ")
                .Element("Title")
                .FirstNode
                .ReplaceWith("Here we go (modified) ");
            xDoc.Descendants("MusicTrack").ToList().ForEach(x => Console.WriteLine(x.Value));

            //insert a node
            xDoc.Descendants("MusicTrack")
                .Where(x => x.Element("Genre") == null)
                .ToList()
                .ForEach(y => y.Add(new XElement("Genre", "(added genre Indie)")));
            xDoc.Descendants("MusicTrack").ToList().ForEach(x => Console.WriteLine(x.Value));
        }
    }
}
