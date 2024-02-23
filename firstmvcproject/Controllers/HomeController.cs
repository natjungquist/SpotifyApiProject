using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using firstmvcproject.Models.ServiceModels.Interfaces;
using firstmvcproject.Models.DataModels;
using firstmvcproject.Models.DataTransferModels;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using Microsoft.AspNetCore.Http.Features;
using firstmvcproject.Models.DataModels.Responses;
using static SpotifyAPI.Web.PlaylistRemoveItemsRequest;
using SpotifyAPI.Web;

namespace firstmvcproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly ISpotifyService _spotifyService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memory;

        public HomeController(ISpotifyAccountService spotifyAccountService, IConfiguration configuration, ISpotifyService spotifyService, ILogger<HomeController> logger, IMemoryCache memory)
        {
            _spotifyAccountService= spotifyAccountService;
            _configuration = configuration;
            _spotifyService= spotifyService;
            _logger = logger;
            _memory = memory;
        }

        
        public async Task<IActionResult> InitialLoad()
        {
            var token = TempData["Token"] as string;
            if (token == null)
            {
                throw new Exception("Access token is null");
            }
            
            User user = new User();
            user.AuthAccessToken = token;
            user.UserProfile = await _spotifyService.GetProfile(user.AuthAccessToken);

            //user.UserPlaylists = await _spotifyService.GetUserPlaylists(clientId, clientSecret, userid, user.AuthAccessToken);

            user.UserTopTracks = await _spotifyService.GetTopTracks(user.AuthAccessToken);

            user.GenresDict = await MakeGenresAndTracksChain(user);

            _memory.Set("User", user, TimeSpan.FromMinutes(60));

            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Should only be entering this method after having generated an access token.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            if (!_memory.TryGetValue("User", out User user))
            {
                return RedirectToAction("RequestSpotifyUserAuthorization", "Auth");
            }
            
            List<string> allGenres = user.GenresDict.GetKeys();
            int numGenres = allGenres.Count;
            //List<string> topGenres = allGenres.Take(10)ToList();

            //get all songs by this genre = genresDict.GetValues()
            //nums songs per this genre = genresDict.GetValue().Count or something
            //get all songs except this genre(s)

            HomeIndexView homeIndexView = new()
            {
                UserProfile = user.UserProfile,
                UserPlaylists = user.UserPlaylists,
                UserTopTracks = user.UserTopTracks,
                TotalGenres = numGenres,
                AllGenres = allGenres
            };
            return View(homeIndexView);
        }

        public async Task<ChainingTable> MakeGenresAndTracksChain(User user)
        {
            ChainingTable genresDict = new();
            foreach (Track track in user.UserTopTracks.items)
            {
                foreach (Artist artist in track.artists)
                {
                    Artist thisArtist = await _spotifyService.GetArtist(user.AuthAccessToken, artist.id);
                    foreach (string genre in thisArtist.genres)
                    {
                        genresDict.Add(genre, track);
                    }
                }
            }
            return genresDict;
        }

        public IActionResult FilterByGenre(List<string> selectedGenres)
        {
            if (!_memory.TryGetValue("User", out User user))
            {
                return RedirectToAction("RequestSpotifyUserAuthorization", "Auth");
            }

            ChainingTable filteredTracks = FilterOutGenres(selectedGenres, user.GenresDict);

            //could prolly make a partial view for this
            HomeIndexView homeIndexView = new()
            {
                UserProfile = user.UserProfile,
                UserPlaylists = user.UserPlaylists,
                UserTopTracks = user.UserTopTracks,
                FilteredTracks = filteredTracks
            };
            return View("Index", homeIndexView);
        }

        public ChainingTable FilterOutGenres(List<string> selectedGenres, ChainingTable genresDict)
        {
            ChainingTable result = new();

            List<string> allGenres = genresDict.GetKeys();
            List<string> filteredGenres = new List<string>(allGenres);

            //filteredGenres = remove selectedGenres from allGenres
            foreach (string g in allGenres)
            {
                filteredGenres.Remove(g);
            }

            foreach (string genre in filteredGenres)
            {
                foreach (Track track in genresDict.GetValues(genre))
                {
                    result.Add(genre, track);
                }
            }
            return result;
        }

        public IActionResult Logout()
        {

            return RedirectToAction("Login", "Auth");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}