using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Musicus.Helpers;
using Musicus.Managers;
using Musicus.Models;

namespace Musicus
{
	public class MusicusHub : Hub
	{
		private SignalRHelper _helper;
		private PlayerManager _playerManager;

		public MusicusHub(SignalRHelper helper, PlayerManager playerManager)
		{
			_helper = helper;
			_playerManager = playerManager;
		}

		public override Task OnConnectedAsync()
		{
			var currentTrack = Playlist.GetCurrentTrack();

			if (currentTrack != null)
			{
				_helper.SetVolume(_playerManager.GetVolumeAsync(currentTrack.TrackSource).Result);
				_helper.SetStatus(_playerManager.GetStatusAsync(currentTrack.TrackSource).Result);
			}
			else
			{
				// Default status when nothing is playing..
				_helper.SetStatus("Musicus", "Let there be songs to fill the air", 0, 0, null, false, null);
			}

			_helper.SetPlaylist(Playlist.GetPlaylist());

			return base.OnConnectedAsync();
		}
	}
}
