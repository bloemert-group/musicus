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
		private static float? _currentVolume;
		public static float CurrentVolume
		{
			get
			{
				if (_currentVolume == null)
				{
					// default value = 30
					_currentVolume = 30;
				}
				return _currentVolume.Value;
			}
			set => _currentVolume = value;
		}

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

			if (nextTrack == null) return false;

			var musicService = _musicServices.GetMusicService(nextTrack.TrackSource);

			return await musicService.PlayAsync(nextTrack.Url);
		}

		public async Task<bool> PauseTrackAsync(Track track)
		{
			var musicService = _musicServices.GetMusicService(track.TrackSource);

			return await musicService.PauseAsync();
		}

		public async Task<IMusicServiceStatus> GetStatusAsync(Track currentTrack)
		{
			var musicService = _musicServices.GetMusicService(currentTrack.TrackSource);

			var status = await musicService.GetStatusAsync().ConfigureAwait(false);

			// End of song, play next
			if ((!currentTrack.IsPlaying && !status.IsPlaying) ||
					(currentTrack.Artist == status.Artist && currentTrack.Description == status.Track && status.IsPlaying && status.Current >= (status.Length - 2)))
			{
				await PlayNextTrackAsync();

				// Fetch status again
				return await musicService.GetStatusAsync().ConfigureAwait(false);
			}
			return status;
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

		public float GetVolume() => CurrentVolume;

		public async Task SetVolumeAsync(VolumeFilter volumeFilter)
		{
			var musicService = _musicServices.GetMusicService(volumeFilter.TrackSource);

			CurrentVolume = volumeFilter.Volume;

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
