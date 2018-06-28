using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;
using Musicus.FileSystemService.Helpers;

namespace Musicus.FileSystemService
{
	public class FileSystemMusicService : IMusicService
	{
		public FileSystemMusicService(string filePath)
		{
			FileSystemHelper.FilePath = filePath;

			FileSystemHelper.VlcPlayer.EndReached += (obj, args) =>
			{
				OnTrackEnd?.Invoke();
			};
		}

		public TrackSource TrackSource => TrackSource.FileSystem;

		public event Action OnTrackEnd;

		public Task<IActionResult<IMusicServiceStatus>> GetStatusAsync()
		{
			var result = FileSystemHelper.GetStatus();

			return Task.FromResult(result);
		}

		public Task<IActionResult<float>> GetVolumeAsync()
		{
			var result = FileSystemHelper.GetVolume();

			return Task.FromResult(result);
		}

		public Task<IActionResult<object>> NextAsync(string url)
		{
			var result = FileSystemHelper.Play(url);

			return Task.FromResult(result);
		}

		public Task<IActionResult<object>> PauseAsync()
		{
			var result = FileSystemHelper.Pause();

			return Task.FromResult(result);
		}

		public Task<IActionResult<object>> PlayAsync()
		{
			var result = FileSystemHelper.Play();

			return Task.FromResult(result);
		}

		public Task<IActionResult<object>> PlayAsync(string url)
		{
			var result = FileSystemHelper.Play(url);

			return Task.FromResult(result);
		}

		public Task<IActionResult<IList<ISearchResult>>> SearchAsync(string keyword)
		{
			var result = FileSystemHelper.Search(keyword);

			return Task.FromResult(result);
		}

		public Task<IActionResult<float>> SetVolumeAsync(float volume)
		{
			var result = FileSystemHelper.SetVolume(volume);

			return Task.FromResult(result);
		}
	}
}
