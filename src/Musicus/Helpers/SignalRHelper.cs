using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Musicus.Models;
using System.Threading;
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
			SetStatus(spotifyStatus.Artist, spotifyStatus.Track, spotifyStatus.Current, spotifyStatus.Length, spotifyStatus.AlbumArtWork, spotifyStatus.IsPlaying);
		}

		public void SetStatus(string artist, string track, double current, double length, string albumArtWork, bool play)
		{
			_hubContext.Clients.All.SendAsync("SetStatus", new { Artist = artist, Track = track, Current = current, Length = length, albumArtWork = albumArtWork, Play = play });
		}

		public void StartStatusUpdate()
		{
			var t = new Thread(() =>
			{
				while (true)
				{
					SetSpotifyStatus(SpotifyHelper.GetStatus());

					Thread.Sleep(1000);
					//your infinite loop 
				}
			});

			t.Start();
		}
	}
}
