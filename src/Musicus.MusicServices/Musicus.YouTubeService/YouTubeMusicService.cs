using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;
using Musicus.YouTubeService.Helpers;

namespace YouTubeService
{
	public class YouTubeMusicService : IMusicService
	{
		public YouTubeMusicService()
		{
			YouTubeHelper.VlcPlayer.EndReached += (obj, args) =>
			{
				OnTrackEnd?.Invoke();
			};
		}

		public TrackSource TrackSource => TrackSource.YouTube;

		public Task<IActionResult<IMusicServiceStatus>> GetStatusAsync()
		{
			var result = YouTubeHelper.GetStatus();

			return Task.FromResult(result);
		}

		public Task<IActionResult<float>> GetVolumeAsync()
		{
			var result = YouTubeHelper.GetVolume();

			return Task.FromResult(result);
		}

		public Task<IActionResult<object>> NextAsync(string url)
			 => YouTubeHelper.PlayAsync(url);

		public Task<IActionResult<object>> PauseAsync()
		{
			var result = YouTubeHelper.Pause();

			return Task.FromResult(result);
		}

		public Task<IActionResult<object>> PlayAsync()
		{
			var result = YouTubeHelper.Play();

			return Task.FromResult(result);
		}

		public Task<IActionResult<object>> PlayAsync(string url)
			 => YouTubeHelper.PlayAsync(url);

		public Task<IActionResult<IList<ISearchResult>>> SearchAsync(string keyword)
		 => YouTubeHelper.SearchAsync(keyword);

		public Task<IActionResult<float>> SetVolumeAsync(float volume)
		{
			var result = YouTubeHelper.SetVolume(volume);

			return Task.FromResult(result);
		}

		public Task<IActionResult<bool>> StopAsync()
		{
			var result = YouTubeHelper.Stop();

			return Task.FromResult(result);
		}

		public event Action OnTrackEnd;
	}
}
