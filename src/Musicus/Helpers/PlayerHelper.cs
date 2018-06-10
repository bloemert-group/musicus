using System;
using Musicus.Models;

namespace Musicus.Helpers
{
	public static class PlayerHelper
	{
		public static bool Play(Track track)
		{
			if (track == null || string.IsNullOrEmpty(track.TrackSource) || string.IsNullOrEmpty(track.Description))
			{
				return PlayNextTrack();
			}

			switch (track.TrackSource)
			{
				default:
				case "Spotify":
					return SpotifyHelper.Play();
			}
		}

		public static bool PlayNextTrack()
		{
			var nextTrack = Playlist.GetNextTrack();

			switch (nextTrack.TrackSource)
			{
				default:
				case "Spotify":
					return SpotifyHelper.Play(nextTrack.Url);
				case "YouTube":
					throw new NotImplementedException("YouTube is not supported (yet)");
			}
		}

		public static bool PauseTrack(Track track)
		{
			switch (track.TrackSource)
			{
				default:
				case "Spotify":
					SpotifyHelper.Pause();
					return true;
			}
		}
	}
}
