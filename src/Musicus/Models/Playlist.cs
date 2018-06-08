using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Musicus.Models
{
	public class Playlist
	{
		public ConcurrentDictionary<int, PlaylistItem> Items { get; set; } = new ConcurrentDictionary<int, PlaylistItem>();

		private static Playlist _instance;
		protected static Playlist Instance => _instance ?? (_instance = new Playlist());

		public static bool AddItemToList(PlaylistItem item)
		{
			var playlist = Playlist.Instance.Items;

			var index = playlist.Count;
			return playlist.TryAdd(index, item);
		}

		public static IList<PlaylistItem> GetPlaylist(bool includingPlayed = false)
		{
			if (includingPlayed)
			{
				return Playlist.Instance.Items.Values.ToList();
			}

			return Playlist.Instance.Items.Values.Where(track => !track.Played).ToList();
		}

		public static PlaylistItem GetNextTrack()
		{
			var playlist = GetPlaylist();

			var nextTrack = playlist.FirstOrDefault();

			Playlist.SetTrackToPlayed(nextTrack);
			return nextTrack;
		}

		public static void SetTrackToPlayed(PlaylistItem track)
		{
			var playlist = GetPlaylist();
			var trackIndex = playlist.IndexOf(track);

			track.Played = true;
			Playlist.Instance.Items.TryUpdate(trackIndex, track, playlist[trackIndex]);
		}

		public static void RemoveTrackFromPlaylist(PlaylistItem track)
		{
			var playlist = GetPlaylist();
			var trackIndex = playlist.IndexOf(track);

			Playlist.Instance.Items.TryRemove(trackIndex, out var removedTrack);
		}
	}
}
