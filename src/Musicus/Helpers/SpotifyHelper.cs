using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Musicus.Models;
using Musicus.Models.Spotify;
using Newtonsoft.Json;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;

namespace Musicus.Helpers
{
	public class SpotifyHelper
	{
		private static string _clientId = "2f68bc04dcf14e118b3f5e8ffe3bdcd7";
		private static string _clientSecret = "79b758453b114fcc96d402cbfc7969d0";

		private static SpotifyLocalAPI _spotifyAPi;
		private static SpotifyLocalAPI SpotifyAPI
		{
			get
			{
				if (_spotifyAPi == null)
				{
					_spotifyAPi = new SpotifyLocalAPI();
					_spotifyAPi.Connect();
				}
				return _spotifyAPi;
			}
		}

		private static SpotifyWebAPI _spotifyWebAPI;
		private static SpotifyWebAPI SpotifyWebAPI
		{
			get
			{
				if (_spotifyWebAPI == null)
				{
					var tokenObj = GetClientCredentialsAuthToken();
					_spotifyWebAPI = new SpotifyWebAPI
					{
						AccessToken = tokenObj.TokenType + " " + tokenObj.AccessToken
					};
				}
				return _spotifyWebAPI;
			}

		}

		private static AuthorizationModel GetClientCredentialsAuthToken()
		{
			var webClient = new WebClient();

			var postparams = new NameValueCollection();
			postparams.Add("grant_type", "client_credentials");

			var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{_clientId}:{_clientSecret}"));
			webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

			var tokenResponse = webClient.UploadValues("https://accounts.spotify.com/api/token", postparams);

			var textResponse = Encoding.UTF8.GetString(tokenResponse);

			return JsonConvert.DeserializeObject<AuthorizationModel>(textResponse);
		}

		public static bool Play(string spotifyUrl = "")
		{
			if (!string.IsNullOrEmpty(spotifyUrl))
			{
				Task.Run(() => SpotifyAPI.PlayURL(spotifyUrl));
			}
			else
			{
				Task.Run(() => SpotifyAPI.Play());
			}

			return true;
		}
		public static void Pause() => Task.Run(() => SpotifyAPI.Pause());

		public static void Previous() => SpotifyAPI.Previous();

		public static void Next() => SpotifyAPI.Skip();

		public static float GetVolume()
		{
			return SpotifyAPI.GetSpotifyVolume();
		}
		public static void SetVolume(float volume)
		{
			SpotifyAPI.SetSpotifyVolume(volume);
		}

		public static SpotifyStatus GetStatus()
		{
			var result = new SpotifyStatus();

			var status = SpotifyAPI.GetStatus();
			if (status != null)
			{
				result.IsPlaying = status.Playing;
				result.Artist = status.Track?.ArtistResource?.Name;
				result.Track = status.Track?.TrackResource?.Name;
				result.Length = status.Track != null ? status.Track.Length : 0;
				result.Current = status.PlayingPosition;
				result.TrackSource = "Spotify";
				if (status.Track != null && status.Track.AlbumResource != null)
				{
					result.AlbumArtWork = status.Track.GetAlbumArtUrl(AlbumArtSize.Size160);
				}
			}

			return result;
		}

		public static List<SearchResult> Search(string keyword)
		{
			var result = new List<SearchResult>();

			var searchItem = SpotifyWebAPI.SearchItems(keyword, SearchType.Track);
			foreach (var t in searchItem.Tracks.Items)
			{
				result.Add(new SearchResult
				{
					Artist = t.Artists[0].Name,
					Description = t.Name,
					TrackId = t.Uri,
					TrackLength = t.DurationMs,
					Type = SearchResultType.Track,
					Url = t.Uri,
					TrackSource = "Spotify"
				});
			}

			searchItem = SpotifyWebAPI.SearchItems(keyword, SearchType.Playlist);
			foreach (var p in searchItem.Playlists.Items)
			{
				result.Add(new SearchResult
				{
					TrackId = p.Id,
					Description = p.Name,
					TrackCount = p.Tracks.Total,
					Type = SearchResultType.Playlist,
					Url = p.Uri,
					TrackSource = "Spotify"
				});
			}

			return result;
		}
	}
}
