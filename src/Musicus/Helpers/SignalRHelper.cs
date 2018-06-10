using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using Musicus.Models;
using Musicus.Models.Spotify;

namespace Musicus.Helpers
{
	public class SignalRHelper
	{
		private IHubContext<MusicusHub> _hubContext;

		public SignalRHelper(IHubContext<MusicusHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public void SetVolume(float volume)
		{
			_hubContext.Clients.All.SendAsync("SetVolume", volume);
		}

		public void SetSpotifyStatus(SpotifyStatus spotifyStatus)
		{
			SetStatus(spotifyStatus.Artist, spotifyStatus.Track, spotifyStatus.Current, spotifyStatus.Length, spotifyStatus.AlbumArtWork, spotifyStatus.IsPlaying, spotifyStatus.TrackSource);
		}

		public void SetStatus(string artist, string track, double current, double length, string albumArtWork, bool play, string trackSource)
		{
			_hubContext.Clients.All.SendAsync("SetStatus", new { Artist = artist, Track = track, Current = current, Length = length, albumArtWork = albumArtWork, Play = play, TrackSource = trackSource });
		}

		public void SetPlaylist(IList<Track> playlist)
		{
			_hubContext.Clients.All.SendAsync("SetQueue", playlist);
		}

		public void StartStatusUpdate()
		{
			var t = new Thread(() =>
			{
				while (true)
				{
					SetSpotifyStatus(SpotifyHelper.GetStatus());
					SetPlaylist(Playlist.GetPlaylist());

					Thread.Sleep(1000);
					//your infinite loop 
				}
			});

			t.Start();
		}
	}
}
