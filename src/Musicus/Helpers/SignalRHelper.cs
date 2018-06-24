using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Musicus.Abstractions.Models;
using Musicus.Models;

namespace Musicus.Helpers
{
	public class SignalRHelper
	{
		private IHubContext<MusicusHub> _hubContext;
		private readonly Player _player;

		protected IMusicServiceStatus DefaultStatus => new MusicServiceStatus
		{
			Artist = "Musicus",
			Track = "Let there be songs to fill the air",
			TrackSource = TrackSource.Spotify,
			AlbumArtWork = "http://icons.iconarchive.com/icons/webalys/kameleon.pics/64/Cloud-Music-icon.png"
		};

		public SignalRHelper(IHubContext<MusicusHub> hubContext, Player player)
		{
			_hubContext = hubContext;
			_player = player;
		}

		public void SetVolume(float volume)
			=> _hubContext.Clients.All.SendAsync("SetVolume", volume);

		public void SetStatus(IMusicServiceStatus status)
			=> SetStatus(status.Artist, status.Track, status.Current, status.Length, status.AlbumArtWork, status.IsPlaying, status.TrackSource);

		public void SetStatus(string artist, string track, double current, double length, string albumArtWork, bool play, TrackSource? trackSource)
			=> _hubContext.Clients.All.SendAsync(
				"SetStatus",
				new
				{
					Artist = artist,
					Track = track,
					Current = current,
					Length = length,
					AlbumArtWork = albumArtWork,
					Play = play,
					TrackSource = trackSource
				});

		public void SetPlaylist(IList<Track> playlist)
			=> _hubContext.Clients.All.SendAsync("SetQueue", playlist);

		public async Task StartStatusUpdate()
		{
			while (true)
			{
				await StatusUpdateAsync();

				await Task.Delay(1000);
			}
		}

		public async Task StatusUpdateAsync()
		{
			var currentTrack = Playlist.GetCurrentTrack();
			IMusicServiceStatus status;

			if (currentTrack != null && (status = await _player.GetStatusAsync(currentTrack)) != null)
			{
				SetStatus(status);
			}
			else
			{
				SetStatus(DefaultStatus);
			}

			SetPlaylist(Playlist.GetPlaylist());
		}
	}
}
