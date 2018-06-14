using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Musicus.Helpers;
using Musicus.Managers;

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

		public override async Task OnConnectedAsync()
		{
			await _helper.StatusUpdateAsync();
			_helper.SetVolume(_playerManager.GetVolume());

			await base.OnConnectedAsync();
		}
	}
}
