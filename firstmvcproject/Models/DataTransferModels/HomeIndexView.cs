using firstmvcproject.Models.DataModels;
using firstmvcproject.Models.DataModels.Responses;
using System.ComponentModel.DataAnnotations;

namespace firstmvcproject.Models.DataTransferModels
{ public class HomeIndexView
    {
        public UserProfile? UserProfile { get; set; }
        public UserPlaylists? UserPlaylists { get; set; }
        public UserTopTracks? UserTopTracks { get; set; }
        public int? TotalGenres { get; set; }
        public List<string>? AllGenres { get; set; }
        public List<string>? TopGenres { get; set; }
        public ChainingTable? FilteredTracks { get; set; }
    }
   
}
