using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;
using Musicus.Helpers;

namespace SpotifyService
{
	public class SpotifyMusicService : IMusicService
	{
		public TrackSource TrackSource => TrackSource.Spotify;

		public async Task<IMusicServiceStatus> GetStatusAsync() => await Task.Run(() => SpotifyHelper.GetStatus());

		public async Task<float> GetVolumeAsync() => await Task.Run(() => SpotifyHelper.GetVolume());

		public async Task<bool> NextAsync(string url)
		{
			await Task.Run(() => SpotifyHelper.Next(url));

			return true;
		}

		public async Task<bool> PauseAsync()
		{
			await Task.Run(() => SpotifyHelper.Pause());

			return true;
		}

		public async Task<bool> PlayAsync() => await Task.Run(() => SpotifyHelper.Play());

		public async Task<bool> PlayAsync(string url) => await Task.Run(() => SpotifyHelper.Play(url));

		public async Task<IList<ISearchResult>> SearchAsync(string keyword) => await Task.Run(() => SpotifyHelper.Search(keyword));

		public async Task SetVolumeAsync(int volume) => await Task.Run(() => SpotifyHelper.SetVolume(volume));
	}
}
