using firstmvcproject.Models.DataModels.Responses;

namespace firstmvcproject.Models.ServiceModels.Interfaces
{
    public interface ISpotifyService
    {
        Task<UserProfile> GetProfile(string accessToken);
        Task<UserTopTracks> GetTopTracks(string accessToken);
        Task<Artist> GetArtist(string accessToken, string artistId);

        Task<UserPlaylists> GetUserPlaylists(string userid, string accessToken);

    }
}
