using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;
using Musicus.Helpers;

namespace SpotifyService
{
	public class SpotifyMusicService : IMusicService
	{
		public SpotifyMusicService(string clientId, string clientSecret)
		{
			SpotifyHelper.ClientId = clientId;
			SpotifyHelper.ClientSecret = clientSecret;
		}

		public TrackSource TrackSource => TrackSource.Spotify;

		public Task<IActionResult<IMusicServiceStatus>> GetStatusAsync() => Task.Run(() => SpotifyHelper.GetStatus());

		public Task<IActionResult<float>> GetVolumeAsync() => Task.Run(() => SpotifyHelper.GetVolume());

		public Task<IActionResult<object>> NextAsync(string url) => Task.Run(() => SpotifyHelper.Next(url));

		public Task<IActionResult<object>> PauseAsync() => Task.Run(() => SpotifyHelper.Pause());

		public Task<IActionResult<object>> PlayAsync() => Task.Run(() => SpotifyHelper.Play());

		public Task<IActionResult<object>> PlayAsync(string url) => Task.Run(() => SpotifyHelper.Play(url));

		public Task<IActionResult<IList<ISearchResult>>> SearchAsync(string keyword) => Task.Run(() => SpotifyHelper.Search(keyword));

		public Task<IActionResult<float>> SetVolumeAsync(float volume) => Task.Run(() => SpotifyHelper.SetVolume(volume));
	}
}
