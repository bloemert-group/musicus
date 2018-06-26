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

			SpotifyHelper.SpotifyAPI.OnTrackTimeChange += (obj, args) =>
			{
				var status = SpotifyHelper.GetStatus();

				if (status.Data.Length > 0 && (args.TrackTime == 0 || (status.Data.Length - 0.2) <= args.TrackTime))
				{
					OnTrackEnded();
				}
			};
		}

		public TrackSource TrackSource => TrackSource.Spotify;

		public Task<IActionResult<IMusicServiceStatus>> GetStatusAsync() => Task.Run(() => SpotifyHelper.GetStatus());

		public Task<IActionResult<float>> GetVolumeAsync() => Task.Run(() => SpotifyHelper.GetVolume());

		public Task<IActionResult<object>> NextAsync(string url) => SpotifyHelper.NextAsync(url);

		public Task<IActionResult<object>> PauseAsync() => SpotifyHelper.PauseAsync();

		public Task<IActionResult<object>> PlayAsync() => SpotifyHelper.PlayAsync();

		public Task<IActionResult<object>> PlayAsync(string url) => SpotifyHelper.PlayAsync(url);

		public Task<IActionResult<IList<ISearchResult>>> SearchAsync(string keyword) => Task.Run(() => SpotifyHelper.Search(keyword));

		public Task<IActionResult<float>> SetVolumeAsync(float volume) => Task.Run(() => SpotifyHelper.SetVolume(volume));

		public event TrackEndHandler TrackEndedEvent;
		public void OnTrackEnded()
		{
			TrackEndedEvent?.Invoke();
		}
	}
}
