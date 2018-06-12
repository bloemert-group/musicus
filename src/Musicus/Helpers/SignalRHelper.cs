using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using Musicus.Models;

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

		public void SetStatus(MusicServiceStatus status)
		{
			SetStatus(status.Artist, status.Track, status.Current, status.Length, status.AlbumArtWork, status.IsPlaying, status.TrackSource);
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
					var currentTrack = Playlist.GetCurrentTrack();

					if (currentTrack != null)
					{
						SetStatus(PlayerHelper.GetStatusAsync(currentTrack.TrackSource).Result);
					}

					SetPlaylist(Playlist.GetPlaylist());

					Thread.Sleep(1000);
					//your infinite loop 
				}
			});

			t.Start();
		}
	}
}
