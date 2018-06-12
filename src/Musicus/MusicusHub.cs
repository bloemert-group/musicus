using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Musicus.Helpers;
using Musicus.Models;

namespace Musicus
{
	public class MusicusHub : Hub
	{
		private SignalRHelper _helper;
		private PlayerHelper _playerHelper;

		public MusicusHub(SignalRHelper helper, PlayerHelper playerHelper)
		{
			_helper = helper;
			_playerHelper = playerHelper;
		}

		public override Task OnConnectedAsync()
		{
			var currentTrack = Playlist.GetCurrentTrack();

			if (currentTrack != null)
			{
				_helper.SetVolume(_playerHelper.GetVolumeAsync(currentTrack.TrackSource).Result);
				_helper.SetStatus(_playerHelper.GetStatusAsync(currentTrack.TrackSource).Result);
			}

			_helper.SetPlaylist(Playlist.GetPlaylist());

			return base.OnConnectedAsync();
		}
	}
}
