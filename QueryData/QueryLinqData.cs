using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryData
{
    public class QueryLinqData
    {
        public void Query() => new QueryLinq().GetAllTracksFromArtist(new SeedGenerator().Seed(), "Bear Bearrison");
        public void Projection() => new QueryLinq().Projection(new SeedGenerator().Seed(), "Bear Bearrison");
        public void Join() => new QueryLinq().Join(new SeedGenerator().Seed(), new SeedGenerator().SeedArtists(), "Bearearo Rocher");
        public void Group() => new QueryLinq().Group(new SeedGenerator().Seed());
    }
    public class QueryLinq
    {
        public void GetAllTracksFromArtist(List<MusicTrack> musicTracks, string name)
        {
            IEnumerable<MusicTrack> tracks = from track in musicTracks where track.Artist.Name.Equals(name) select track;
            //OR
            IEnumerable<MusicTrack> tracks1 = musicTracks.Where(x => x.Artist.Name.Equals(name));
            tracks.ToList().ForEach(x => Console.WriteLine(x.Title));
            tracks1.ToList().ForEach(x => Console.WriteLine(x.Title));
        }
        public void Projection(List<MusicTrack> musicTracks, string name)
        {
            var projectToAnnonymous = from track in musicTracks
                                      where track.Artist.Name.Equals(name)
                                      select
                                            new
                                            {
                                                AristName = track.Artist.Name,
                                                TitleName = track.Title,
                                                Genre = track.Genres
                                            };

            //OR
            var projectToAnnonymous1 = musicTracks.Where(x => x.Artist.Name.Equals(name)).Select(y => new
            {
                AristName = y.Artist.Name,
                TitleName = y.Title,
                Genre = y.Genres
            });
            projectToAnnonymous
                .ToList()
                .ForEach(x => Console.WriteLine($"Artist: {x.AristName} - title {x.TitleName} - genre {x.Genre.ToList().Select(g => g.Name).First()}"));
            Console.WriteLine();
            Console.WriteLine();
            projectToAnnonymous1
                .ToList()
                .ForEach(x => Console.WriteLine($"Artist: {x.AristName} - title {x.TitleName} - genre {x.Genre.ToList().Select(g => g.Name).First()}"));
        }
        public void Join(List<MusicTrack> musicTracks, List<Artist> artists, string name)
        {
            var artistTracks = from artist in artists
                               where artist.Name.Equals(name)
                               join track in musicTracks on artist.Id equals track.ArtistId
                               select new
                               {
                                   ArtistName = artist.Name,
                                   track.Title
                               };

            //OR
            artists
                 .Where(x => x.Name.Equals(name))
                 .Join(musicTracks, //join with 
                         a => a.Id, //on what
                         m => m.ArtistId, //with what //Intellisense bug here.
                         (a, m) => (a.Name, m.Title))
                 .GroupBy(y => y.Name, y => y.Title)
                 .Select(aa => new
                 {
                     Id = aa.Key,
                     Count = aa.Sum(c => c.Length)
                 }).ToList().ForEach(x => Console.WriteLine($"{x.Id} - {x.Count}")); 

            artistTracks.ToList().ForEach(Console.WriteLine);
        }
        public void Group(List<MusicTrack> musicTracks)
        {
            var artistSummary = from track in musicTracks
                                group track by track.ArtistId
                                into artistTrackSummary
                                select new
                                {
                                    Id = artistTrackSummary.Key,
                                    Count = artistTrackSummary.Count()
                                };
            artistSummary.ToList().ForEach(x => Console.WriteLine($"Id {x.Id} - Count {x.Count}"));

            //OR
            musicTracks
                .GroupBy(x => x.ArtistId)
                .Select(y => new
                {
                    Id = y.Key,
                    Count = y.Count()
                }).ToList().ForEach(x => Console.WriteLine($"Id {x.Id} - Count {x.Count}"));
        }
    }
    public class SeedGenerator
    {
        public List<MusicTrack> Seed()
        {
            string[] songNames = {  "Stairway to Bananas",
                                    "Total Eclipse of the Teddy Bear",
                                    "Like a Teddy Bear",
                                    "Independent Teddy Bear",
                                    "The Homecoming Queen's Got A Teddy Bear"};
            string[] artistsNames = { "Bear Bearrison", "Bearaello", "Bearero Rocher" };
            string[] genresNames = { "Jazz", "Blues", "Rock", "Indie" };
            List<MusicTrack> musicTracks = new List<MusicTrack>();
            Random rnd = new Random();
            int i = 1;
            foreach (string artistName in artistsNames)
            {
                Artist artist = new Artist() { Id = i, Name = artistName };
                int j = i;
                foreach (string title in songNames)
                {
                    MusicTrack mt = new MusicTrack
                    {
                        ArtistId = j,
                        Artist = artist,
                        SongLength = rnd.Next(3, 5),
                        Title = title,
                        Genres = new List<Genre>() {
                            new Genre() { Name = genresNames[rnd.Next(genresNames.Length - 1)] },
                            new Genre() { Name = genresNames[rnd.Next(genresNames.Length - 1)] },
                        }
                    };
                    musicTracks.Add(mt);
                    j++;
                }
                i++;
            }
            return musicTracks;
        }

        public List<Artist> SeedArtists()
        {
            return new List<Artist>{
                new Artist {Id=1, Name= "Bear Bearrison" },
                new Artist {Id=2, Name= "Bearaello" },
                new Artist {Id=5, Name= "Bearearo Rocher" },
            };
        }
    }
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Genre
    {
        public string Name { get; set; }
    }
    public class MusicTrack
    {
        public int Id { get; set; }
        public MusicTrack()
        {
            Genres = new List<Genre>();
        }
        public string Title { get; set; }
        public int SongLength { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
