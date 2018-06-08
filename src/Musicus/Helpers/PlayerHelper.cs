using System;
using Musicus.Models;

namespace Musicus.Helpers
{
	public static class PlayerHelper
	{
		public static bool PlayNextTrack()
		{
			var nextTrack = Playlist.GetNextTrack();

			switch (nextTrack.TrackSource)
			{
				default:
				case "Spotify":
					return SpotifyHelper.Play(nextTrack.Url);
				case "YouTube":
					throw new NotImplementedException("YouTube wordt nog niet ondersteund");
			}
		}
	}
}
