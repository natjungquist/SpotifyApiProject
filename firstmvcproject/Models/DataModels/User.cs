using System.ComponentModel.DataAnnotations;
using firstmvcproject.Models.DataModels.Responses;

namespace firstmvcproject.Models.DataModels
{
    public class User
    {
        [Required]
        public string? AuthAccessToken { get; set; }

        public string? UserId { get; set; }

        public UserProfile? UserProfile { get; set; }

        public UserTopTracks? UserTopTracks { get; set; }

        public UserTopArtists? UserTopArtists { get; set; }

        public UserPlaylists? UserPlaylists { get; set; }
        public ChainingTable? GenresDict { get; set; }

    }
}
