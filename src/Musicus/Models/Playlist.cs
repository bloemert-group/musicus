using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Musicus.Models
{
	public class Playlist
	{
		public ConcurrentDictionary<int, Track> Items { get; set; } = new ConcurrentDictionary<int, Track>();

		private static Playlist _instance;
		protected static Playlist Instance => _instance ?? (_instance = new Playlist());

		public static bool AddItemToList(Track item)
		{
			var playlist = Playlist.Instance.Items;

			var index = playlist.Count;
			return playlist.TryAdd(index, item);
		}

		public static IList<Track> GetPlaylist(bool includingPlayed = false, bool includingIsPlaying = true)
		{
			var result = new List<Track>();
			result.AddRange(Playlist.Instance.Items.Values.ToList());

			if (!includingPlayed)
			{
				result.RemoveAll(tr => tr.Played);
			}
			if (!includingIsPlaying)
			{
				result.RemoveAll(tr => tr.IsPlaying);
			}

			return result;
		}

		public static Track GetNextTrack()
		{
			var playlist = GetPlaylist();

			var nextTrack = playlist.FirstOrDefault(tr => !tr.IsPlaying);
			var currentTrack = GetCurrentTrack();

			if (currentTrack != null)
			{
				Playlist.SetTrackToPlayed(currentTrack);
			}
			if (nextTrack != null)
			{
				nextTrack.IsPlaying = true;
			}

			return nextTrack;
		}

		public static void SetTrackToPlayed(Track track)
		{
			if (track == null) return;

			var playlist = GetPlaylist();
			var trackIndex = playlist.IndexOf(track);

			track.Played = true;
			track.IsPlaying = false;
			Playlist.Instance.Items.TryUpdate(trackIndex, track, playlist[trackIndex]);
		}

		public static void RemoveTrackFromPlaylist(Track track)
		{
			var playlist = GetPlaylist();
			var trackIndex = playlist.IndexOf(track);

			Playlist.Instance.Items.TryRemove(trackIndex, out var removedTrack);
		}

		public static Track GetCurrentTrack()
		{
			var playlist = GetPlaylist();

			return playlist.FirstOrDefault(track => track.IsPlaying);
		}
	}
}
