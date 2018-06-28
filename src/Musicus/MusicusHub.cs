using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Musicus.Helpers;

namespace Musicus
{
	public class MusicusHub : Hub
	{
		private SignalRHelper _helper;

		public MusicusHub(SignalRHelper helper)
		{
			_helper = helper;
		}

		public override async Task OnConnectedAsync()
		{
			await _helper.StatusUpdateAsync();
			_helper.SetVolume(VolumeHelper.GetVolume());

			await base.OnConnectedAsync();
		}
	}
}
