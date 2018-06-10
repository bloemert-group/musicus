using Microsoft.AspNetCore.Mvc;
using Musicus.Helpers;
using Musicus.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Musicus.ApiControllers
{
	[Route("api/musicus")]
	public class MusicusController : Controller
	{
		private SignalRHelper _signalRHelper;
		public MusicusController(SignalRHelper signalRHelper)
		{
			_signalRHelper = signalRHelper;
		}

		[HttpPost]
		[Route("play")]
		public IActionResult Play([FromBody]Track track)
			=> Json(new { Succeed = PlayerHelper.Play(track) });

		[HttpPost]
		[Route("pause")]
		public IActionResult Pause([FromBody]Track track)
			=> Json(new { Succeed = PlayerHelper.PauseTrack(track) });

		[Route("status")]
		public IActionResult Status()
		{
			return Json(SpotifyHelper.GetStatus());
		}

		[HttpPost]
		[Route("next")]
		public IActionResult Next() => Json(new { Succeed = PlayerHelper.PlayNextTrack() });

		[Route("setvolume/{volume}")]
		public IActionResult SetVolume(float volume)
		{
			SpotifyHelper.SetVolume(volume);

			_signalRHelper.SetVolume(volume);

			return Json(new { Succeed = true });
		}

		[HttpPost]
		[Route("search")]
		public IActionResult Search([FromBody] SearchFilter filter)
		{
			if (string.IsNullOrEmpty(filter.Keyword))
			{
				return null;
			}

			return Json(SpotifyHelper.Search(filter.Keyword));
		}

		[HttpPost]
		[Route("addtoqueue")]
		public IActionResult AddToQueue([FromBody] Track item)
		{
			if (string.IsNullOrEmpty(item.TrackId))
			{
				return Json(new { Succeed = false });
			}

			Playlist.AddItemToList(item);

			return GetPlaylist();
		}

		[HttpGet]
		public IActionResult GetPlaylist()
		{
			var playlist = Playlist.GetPlaylist();

			_signalRHelper.SetPlaylist(playlist);

			return Json(new { Succeed = true, Data = playlist });
		}
	}
}
