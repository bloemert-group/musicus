using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;
using Musicus.Helpers;
using Musicus.Models;

namespace Musicus
{
	public class Player
	{
		public static int DefaultMusicServiceVolumeLevel = 30;
		// TODO: this should be done differently 
		// If it works, it ain't stupid
		public static Action<string> ExceptionHandler;

		private readonly IEnumerable<IMusicService> _musicServices;

		public Player(IEnumerable<IMusicService> musicServices)
		{
			_musicServices = musicServices;

			SetEvents(musicServices);

			VolumeHelper.InitVolume(musicServices, DefaultMusicServiceVolumeLevel);
		}

		private void SetEvents(IEnumerable<IMusicService> musicServices)
		{
			foreach (var ms in musicServices)
			{
				ms.TrackEndedEvent += OnTrackEnd;
			}
		}

		private void OnTrackEnd()
		{
			Task.Run(async () =>
			{
				bool succeed = false;
				string errorMessage;
				// When the next song cannot be played, the next song in queue is played
				// The users will be notified that the song couldn't be played
				while (!succeed && Playlist.GetPlaylist(includingIsPlaying: false).Count > 0)
				{
					(succeed, errorMessage) = await PlayNextTrackAsync();
					if (!succeed)
					{
						ExceptionHandler?.Invoke(errorMessage);
					}
				}
			});
		}

		public async Task<(bool succeed, string errorMessage)> PlayAsync(Track track)
		{
			if (track == null || track.TrackSource == 0 || string.IsNullOrEmpty(track.Description))
			{
				return await PlayNextTrackAsync();
			}

			var musicService = _musicServices.GetMusicService(track.TrackSource);

			var result = await musicService.PlayAsync();

			return (result.Succeed, result.ErrorMessage);
		}

		public async Task<(bool succeed, string errorMessage)> PlayNextTrackAsync()
		{
			var nextTrack = Playlist.GetNextTrack();

			if (nextTrack == null) return (false, "No next track in playlist");

			await _musicServices.PauseAll();

			var musicService = _musicServices.GetMusicService(nextTrack.TrackSource);

			var result = await musicService.PlayAsync(nextTrack.Url);

			return (result.Succeed, result.ErrorMessage);
		}

		public async Task<bool> PauseTrackAsync(Track track)
		{
			var musicService = _musicServices.GetMusicService(track.TrackSource);

			var result = await musicService.PauseAsync();

			return result.Succeed;
		}

		public async Task<IMusicServiceStatus> GetStatusAsync(Track currentTrack)
		{
			try
			{
				var musicService = _musicServices.GetMusicService(currentTrack.TrackSource);

				var statusResult = await musicService.GetStatusAsync().ConfigureAwait(false);

				var status = statusResult?.Data;

				return status;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<IList<ISearchResult>> SearchAsync(SearchFilter filter)
		{
			var taskList = _musicServices.Select(ma => ma.SearchAsync(filter.Keyword));

			var taskResult = await Task.WhenAll(taskList);

			var searchResult = new List<ISearchResult>();
			foreach (var tr in taskResult)
			{
				searchResult.AddRange(tr.Data);
			}
			return searchResult;
		}

		public async Task<float> GetVolumeAsync(TrackSource trackSource)
		{
			var musicService = _musicServices.GetMusicService(trackSource);

			var result = await musicService.GetVolumeAsync();

			return result.Data;
		}

		public void SetVolume(VolumeFilter volumeFilter)
		{
			var musicService = _musicServices.GetMusicService(volumeFilter.TrackSource);

			musicService.SetVolumeAsync(volumeFilter.Volume);
		}
	}

	static class PlayerExtensions
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

		public static async Task PauseAll(this IEnumerable<IMusicService> musicServices)
		{
			var pauseTasks = musicServices.Select(ms => ms.PauseAsync());

			await Task.WhenAll(pauseTasks);
		}
	}
}
