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

		public static IList<Track> GetPlaylist(bool includingPlayed = false)
		{
			if (includingPlayed)
			{
				return Playlist.Instance.Items.Values.ToList();
			}

			return Playlist.Instance.Items.Values.Where(track => !track.Played).ToList();
		}

		public static Track GetNextTrack()
		{
			var playlist = GetPlaylist();

			var nextTrack = playlist.FirstOrDefault();

			Playlist.SetTrackToPlayed(nextTrack);
			return nextTrack;
		}

		public static void SetTrackToPlayed(Track track)
		{
			var playlist = GetPlaylist();
			var trackIndex = playlist.IndexOf(track);

			track.Played = true;
			Playlist.Instance.Items.TryUpdate(trackIndex, track, playlist[trackIndex]);
		}

		public static void RemoveTrackFromPlaylist(Track track)
		{
			var playlist = GetPlaylist();
			var trackIndex = playlist.IndexOf(track);

			Playlist.Instance.Items.TryRemove(trackIndex, out var removedTrack);
		}
	}
}
