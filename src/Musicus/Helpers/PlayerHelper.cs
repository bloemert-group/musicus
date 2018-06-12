using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Musicus.Agents;
using Musicus.Models;

namespace Musicus.Helpers
{
	public static class PlayerHelper
	{
		private static ConcurrentDictionary<TrackSource, IMusicServiceAgent> MusicAgents = new ConcurrentDictionary<TrackSource, IMusicServiceAgent>();

		static PlayerHelper()
		{
			// TODO no hardcoded urls..
			MusicAgents.TryAdd(TrackSource.Spotify, new MusicServiceAgent("http://localhost:49194/"));
			//MusicAgents.TryAdd(TrackSource.YouTube, new MusicServiceAgent(""));
		}

		public static async Task<bool> PlayAsync(Track track)
		{
			if (track == null || track.TrackSource == 0 || string.IsNullOrEmpty(track.Description))
			{
				return await PlayNextTrackAsync();
			}

			if (MusicAgents.TryGetValue(track.TrackSource, out var musicService))
			{
				return await musicService.PlayAsync(track.Url);
			}
			return false;
		}

		public static async Task<bool> PlayNextTrackAsync()
		{
			var nextTrack = Playlist.GetNextTrack();

			if (MusicAgents.TryGetValue(nextTrack.TrackSource, out var musicService))
			{
				return await musicService.PlayAsync(nextTrack.Url);
			}
			return false;
		}

		public static async Task<bool> PauseTrackAsync(Track track)
		{
			if (MusicAgents.TryGetValue(track.TrackSource, out var musicService))
			{
				return await musicService.PlayAsync(track.Url);
			}
			return false;
		}

		public static async Task<MusicServiceStatus> GetStatusAsync(TrackSource trackSource)
		{
			if (MusicAgents.TryGetValue(trackSource, out var musicService))
			{
				return await musicService.GetStatusAsync().ConfigureAwait(false);
			}
			return null;
		}

		public static async Task<IList<SearchResult>> SearchAsync(SearchFilter filter)
		{
			var taskList = MusicAgents.Values.Select(ma => ma.SearchAsync(filter.Keyword));

			var taskResult = await Task.WhenAll(taskList);

			var searchResult = new List<SearchResult>();
			foreach (var tr in taskResult)
			{
				searchResult.AddRange(tr);
			}
			return searchResult;
		}

		public static async Task<int> GetVolumeAsync(TrackSource trackSource)
		{
			if (MusicAgents.TryGetValue(trackSource, out var musicService))
			{
				return await musicService.GetVolumeAsync().ConfigureAwait(false);
			}
			return 1;
		}

		public static async Task SetVolumeAsync(VolumeFilter volumeFilter)
		{
			if (MusicAgents.TryGetValue(volumeFilter.TrackSource, out var musicService))
			{
				await musicService.SetVolumeAsync(volumeFilter.Volume);
			}
		}
	}
}
