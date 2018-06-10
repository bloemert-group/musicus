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
			//helper.SetQueue(QueueItemFactory.Instance.ListQueue().ToList());
			_helper.SetVolume(SpotifyHelper.GetVolume());
			_helper.SetSpotifyStatus(SpotifyHelper.GetStatus());
			_helper.SetPlaylist(Playlist.GetPlaylist());

			return base.OnConnectedAsync();
		}
	}
}
