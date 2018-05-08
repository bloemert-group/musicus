using Microsoft.AspNetCore.SignalR;
using Musicus.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

			return base.OnConnectedAsync();
		}
	}
}
