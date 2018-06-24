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

		public async Task<IActionResult<IMusicServiceStatus>> GetStatusAsync()
			 => await Task.Run(() => YouTubeHelper.GetStatus());

		public async Task<IActionResult<float>> GetVolumeAsync()
			 => await Task.Run(() => YouTubeHelper.GetVolume());

		public async Task<IActionResult<object>> NextAsync(string url)
			 => await YouTubeHelper.PlayAsync(url);

		public async Task<IActionResult<object>> PauseAsync()
			=> await Task.Run(() => YouTubeHelper.Pause());

		public async Task<IActionResult<object>> PlayAsync()
			=> await Task.Run(() => YouTubeHelper.Play());

		public async Task<IActionResult<object>> PlayAsync(string url)
			 => await YouTubeHelper.PlayAsync(url);

		public async Task<IActionResult<IList<ISearchResult>>> SearchAsync(string keyword)
		 => await YouTubeHelper.SearchAsync(keyword);

		public async Task<IActionResult<float>> SetVolumeAsync(float volume)
			=> await Task.Run(() => YouTubeHelper.SetVolume(volume));
	}
}
