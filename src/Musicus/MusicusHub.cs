using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Musicus.Helpers;
using Musicus.Models;

namespace Musicus
{
	public class MusicusHub : Hub
	{
		public SignalRHelper _helper;
		public MusicusHub(SignalRHelper helper)
		{
			_helper = helper;
		}

		public override Task OnConnectedAsync()
		{
			var currentTrack = Playlist.GetCurrentTrack();

			if (currentTrack != null)
			{
				_helper.SetVolume(PlayerHelper.GetVolumeAsync(currentTrack.TrackSource).Result);
				_helper.SetStatus(PlayerHelper.GetStatusAsync(currentTrack.TrackSource).Result);
			}

			_helper.SetPlaylist(Playlist.GetPlaylist());

			return base.OnConnectedAsync();
		}
	}
}
