using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;
using Musicus.SpotifyService.Helpers;

namespace SpotifyService
{
	public class SpotifyMusicService : IMusicService
	{
		public SpotifyMusicService(string clientId, string clientSecret)
		{
			SpotifyHelper.ClientId = clientId;
			SpotifyHelper.ClientSecret = clientSecret;

			SpotifyHelper.SpotifyAPI.OnTrackTimeChange += (obj, args) =>
			{
				var status = SpotifyHelper.GetStatus();

				if (status.Data.Length > 0 && (args.TrackTime == 0 || (status.Data.Length - 0.2) <= args.TrackTime))
				{
					OnTrackEnd?.Invoke();
				}
			};
		}

		public TrackSource TrackSource => TrackSource.Spotify;

		public Task<IActionResult<IMusicServiceStatus>> GetStatusAsync()
		{
			var result = SpotifyHelper.GetStatus();

			return Task.FromResult(result);
		}

		public Task<IActionResult<float>> GetVolumeAsync()
		{
			var result = SpotifyHelper.GetVolume();

			return Task.FromResult(result);
		}

		public Task<IActionResult<object>> NextAsync(string url) => SpotifyHelper.NextAsync(url);

		public Task<IActionResult<object>> PauseAsync() => SpotifyHelper.PauseAsync();

		public Task<IActionResult<object>> PlayAsync() => SpotifyHelper.PlayAsync();

		public Task<IActionResult<object>> PlayAsync(string url) => SpotifyHelper.PlayAsync(url);

		public Task<IActionResult<IList<ISearchResult>>> SearchAsync(string keyword)
		{
			var result = SpotifyHelper.Search(keyword);

			return Task.FromResult(result);
		}

		public Task<IActionResult<float>> SetVolumeAsync(float volume)
		{
			var result = SpotifyHelper.SetVolume(volume);

			return Task.FromResult(result);
		}

		public Task<IActionResult<bool>> StopAsync()
		{
			return SpotifyHelper.StopAsync();
		}

		public event Action OnTrackEnd;
	}
}
