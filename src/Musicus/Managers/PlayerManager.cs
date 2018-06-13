using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;
using Musicus.Models;

namespace Musicus.Managers
{
	public class PlayerManager
	{
		private readonly IEnumerable<IMusicService> _musicServices;

		public PlayerManager(IEnumerable<IMusicService> musicServices)
		{
			_musicServices = musicServices;
		}

		public async Task<bool> PlayAsync(Track track)
		{
			if (track == null || track.TrackSource == 0 || string.IsNullOrEmpty(track.Description))
			{
				return await PlayNextTrackAsync();
			}

			var musicService = _musicServices.GetMusicService(track.TrackSource);

			return await musicService.PlayAsync(track.Url);
		}

		public async Task<bool> PlayNextTrackAsync()
		{
			var nextTrack = Playlist.GetNextTrack();

			var musicService = _musicServices.GetMusicService(nextTrack.TrackSource);

			return await musicService.PlayAsync(nextTrack.Url);
		}

		public async Task<bool> PauseTrackAsync(Track track)
		{
			var musicService = _musicServices.GetMusicService(track.TrackSource);

			return await musicService.PlayAsync(track.Url);
		}

		public async Task<IMusicServiceStatus> GetStatusAsync(TrackSource trackSource)
		{
			var musicService = _musicServices.GetMusicService(trackSource);

			return await musicService.GetStatusAsync().ConfigureAwait(false);
		}

		public async Task<IList<ISearchResult>> SearchAsync(SearchFilter filter)
		{
			var taskList = _musicServices.Select(ma => ma.SearchAsync(filter.Keyword));

			var taskResult = await Task.WhenAll(taskList);

			var searchResult = new List<ISearchResult>();
			foreach (var tr in taskResult)
			{
				searchResult.AddRange(tr);
			}
			return searchResult;
		}

		public async Task<float> GetVolumeAsync(TrackSource trackSource)
		{
			var musicService = _musicServices.GetMusicService(trackSource);

			return await musicService.GetVolumeAsync().ConfigureAwait(false);
		}

		public async Task SetVolumeAsync(VolumeFilter volumeFilter)
		{
			var musicService = _musicServices.GetMusicService(volumeFilter.TrackSource);

			await musicService.SetVolumeAsync(volumeFilter.Volume);
		}
	}

	public static class PlayerHelperExtensions
	{
		public static IMusicService GetMusicService(this IEnumerable<IMusicService> musicServices, TrackSource trackSource)
		{
			var musicService = musicServices.FirstOrDefault(ms => ms.TrackSource == trackSource);

			if (musicService == null)
			{
				throw new ArgumentException(nameof(IMusicService), $"A MusicService with source {trackSource.ToString()} was requested but not implemented");
			}
			return musicService;
		}
	}
}
