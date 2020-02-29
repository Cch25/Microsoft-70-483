using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StoreAndRetrieveDataInCollections
{
    public class CustomCollections
    {
        public void CustomCollectionWithList() => TrackStore.GetTrackStore().RemoveArtist("Bear Bearison");
        public CompassCollection CustomeCollectionWithCollection() => new CompassCollection();
    }

    #region [ Custom lists ]
    public class TrackStore : List<MusicTrack>
    {
        public int RemoveArtist(string name)
        {
            List<MusicTrack> removeList = new List<MusicTrack>();
            foreach (MusicTrack track in this)
            {
                if (track.Artist == name)
                {
                    removeList.Add(track);
                }
            }

            foreach (MusicTrack track in removeList)
            {
                Remove(track);
            }
            Console.WriteLine(this.ToString());
            return removeList.Count;
        }
        public static TrackStore GetTrackStore()
        {
            TrackStore result = new TrackStore();
            string[] artists = new string[] { "Bear Bearison", "Bearaello", "Berrero Rocher" };
            string[] titles = new string[] { "My Way", "Your Way", "His Way", "Her Way" };

            Random rnd = new Random();

            foreach (string artist in artists)
            {
                result.AddRange(from string title in titles
                                let track = new MusicTrack
                                {
                                    Artist = artist,
                                    Title = titles[rnd.Next(0, artists.Length - 1)]
                                }
                                select track);
            }
            return result;
        }
        public override string ToString()
        {
            foreach (MusicTrack track in this)
                Console.WriteLine($"Title:{track.Title} Artist:{track.Artist}");

            return base.ToString();
        }
    }
    public class MusicTrack
    {
        public string Title { get; set; }
        public string Artist { get; set; }
    }
    #endregion

    #region [ Custom collections ]
    public class CompassCollection : ICollection
    {
        private readonly string[] compassPoints = { "N", "S", "W", "E" };

        public int Count => compassPoints.Length;

        // Returns true if the collection is thread safe. This collection is not
        public bool IsSynchronized => false;

        // Returns an object that can be used to synchronise access to this object
        public object SyncRoot => this;

        public void CopyTo(Array array, int index)
        {
            foreach (string point in compassPoints)
            {
                array.SetValue(point, index);
                index += 1;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return compassPoints.GetEnumerator();
        }
    }
    #endregion


}
