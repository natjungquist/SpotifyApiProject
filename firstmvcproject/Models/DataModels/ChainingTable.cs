using firstmvcproject.Models.DataModels.Responses;
using static SpotifyAPI.Web.PlaylistRemoveItemsRequest;

namespace firstmvcproject.Models.DataModels
{
    public class ChainingTable
    {
        private Dictionary<string, List<object>> ht;

        public ChainingTable()
        {
            ht = new();
        }

        public bool isEmpty()
        {
            return ht.Count == 0;
        }
        public int size()
        {
            return ht.Count;
        }
        public void Add(object key, object val)
        {
            if (key is string && val is Track)
            {
                string genre = (string)key;
                Track track = (Track)val;

                if (!ht.ContainsKey(genre))
                {
                    ht.Add(genre, new List<object>());
                }
                ht[genre].Add(track);
            }
            
        }
        public bool ContainsKey(string key)
        {
            return ht.ContainsKey(key);
        }
        public bool ContainsValue(object val)
        {
            foreach (var pair in ht)
            {
                if (pair.Value.Contains(val)) return true;
            }
            return false;
        }
        public string GetKey(object val)
        {
            foreach (var pair in ht)
            {
                if (pair.Value.Contains(val))
                {
                    return pair.Key;
                }
            }
            return null;
        }
        public List<string> GetKeys()
        {
            List<string> allKeys = new();
            foreach (var pair in ht)
            {
                allKeys.Add(pair.Key);
            }
            return allKeys;
        }
        public List<object> GetValues(string key)
        {
            return ht[key];
        }
        public object RemoveValue(object val)
        {
            foreach (var pair in ht)
            {
                if (pair.Value.Contains(val))
                {
                    pair.Value.Remove(val);
                    return val;
                }
            }
            return null;
        }
    }
}
