using firstmvcproject.Models.DataModels.Responses;
using firstmvcproject.Models.ServiceModels.Interfaces;
using System.Text.Json;

namespace firstmvcproject.Models.ServiceModels
{
    public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _httpClient;
        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient; //will be injected at runtime
        }

        public async Task<UserProfile> GetProfile(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", accessToken);

            string uri = "https://api.spotify.com/v1/me";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var profile = await JsonSerializer.DeserializeAsync<UserProfile>(responseStream);
            if (profile != null)
            {
                return profile;
            }
            else
            {
                throw new Exception("error retrieving user profile");
            }
        }

        public async Task<Artist> GetArtist(string accessToken, string artistId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", accessToken);

            string uri = $"https://api.spotify.com/v1/artists/{artistId}";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var artist = await JsonSerializer.DeserializeAsync<Artist>(responseStream);
            if (artist != null)
            {
                return artist;
            }
            else
            {
                throw new Exception("error retrieving artist");
            }
        }

        public async Task<UserTopTracks> GetTopTracks(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                   "Bearer", accessToken);


            //time_range
            //limit
            //offset
            

            string uri = "https://api.spotify.com/v1/me/top/tracks?limit=50&offset=0";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var topTracks = await JsonSerializer.DeserializeAsync<UserTopTracks>(responseStream);
            if (topTracks != null)
            {
                return topTracks;
            }
            else
            {
                throw new Exception("error retrieving top tracks");
            }
        }

        public async Task<UserPlaylists> GetUserPlaylists(string userid, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", accessToken);

            string uri = $"https://api.spotify.com/v1/users/{userid}/playlists";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var playlists = await JsonSerializer.DeserializeAsync<UserPlaylists>(responseStream);
            if (playlists != null)
            {
                return playlists;
            }
            else
            {
                throw new Exception("error retrieving user playlists");
            }
        }

    }
}
