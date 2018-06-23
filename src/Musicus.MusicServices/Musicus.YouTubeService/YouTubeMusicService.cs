using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;
using Musicus.YouTubeService.Helpers;

namespace YouTubeService
{
	public class YouTubeMusicService : IMusicService
	{
		public YouTubeMusicService(string apiKey)
		{
			YouTubeHelper.ApiKey = apiKey;
		}

		public TrackSource TrackSource => TrackSource.YouTube;

		public async Task<IMusicServiceStatus> GetStatusAsync() => await Task.Run(() => YouTubeHelper.GetStatus());

		public Task<bool> NextAsync(string url) => PlayAsync(url);

		public async Task<bool> PauseAsync() => await Task.Run(() => YouTubeHelper.Pause());

		public async Task<bool> PlayAsync() => await Task.Run(() => YouTubeHelper.Play());

		public Task<bool> PlayAsync(string url) => YouTubeHelper.PlayAsync(url);

		public async Task<IList<ISearchResult>> SearchAsync(string keyword) => await YouTubeHelper.SearchAsync(keyword);

		public async Task<bool> SetVolumeAsync(float volume) => await Task.Run(() => YouTubeHelper.SetVolume(volume));

		public async Task<float> GetVolumeAsync() => await Task.Run(() => YouTubeHelper.GetVolume());
	}
}
