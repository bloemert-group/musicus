using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Musicus.Helpers;

namespace Musicus
{
	public class MusicusHub : Hub
	{
		private SignalRHelper _helper;
		private Player _player;

		public MusicusHub(SignalRHelper helper, Player player)
		{
			_helper = helper;
			_player = player;
		}

		public override async Task OnConnectedAsync()
		{
			await _helper.StatusUpdateAsync();
			_helper.SetVolume(_player.GetVolume());

			await base.OnConnectedAsync();
		}
	}
}
