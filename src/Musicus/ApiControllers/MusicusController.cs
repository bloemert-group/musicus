using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Musicus.Abstractions.Models;
using Musicus.Helpers;
using Musicus.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Musicus.ApiControllers
{
	[Route("api/musicus")]
	public class MusicusController : Controller
	{
		private SignalRHelper _signalRHelper;
		private PlayerHelper _playerHelper;

		public MusicusController(SignalRHelper signalRHelper, PlayerHelper playerHelper)
		{
			_signalRHelper = signalRHelper;
			_playerHelper = playerHelper;
		}

		[HttpPost]
		[Route("play")]
		public async Task<IActionResult> PlayAsync([FromBody]Track track)
		{
			var result = await _playerHelper.PlayAsync(track);

			return Json(new { Succeed = true });
		}

		[HttpPost]
		[Route("pause")]
		public async Task<IActionResult> PauseAsync([FromBody]Track track)
		{
			var result = await _playerHelper.PauseTrackAsync(track);

			return Json(new { Succeed = true });
		}

		[HttpGet]
		[Route("status/{tracksource}")]
		public async Task<IActionResult> StatusAsync(TrackSource trackSource)
		{
			var result = await _playerHelper.GetStatusAsync(trackSource);

			return Json(true);
		}

		[HttpPost]
		[Route("next")]
		public async Task<IActionResult> NextAsync()
		{
			var result = await _playerHelper.PlayNextTrackAsync();

			return Json(true);
		}

		[HttpPost]
		[Route("setvolume")]
		public async Task<IActionResult> SetVolumeAsync([FromBody] VolumeFilter volumeFilter)
		{
			await _playerHelper.SetVolumeAsync(volumeFilter);

			_signalRHelper.SetVolume(volumeFilter.Volume);

			return Json(new { Succeed = true });
		}

		[HttpPost]
		[Route("search")]
		public async Task<IActionResult> SearchAsync([FromBody] SearchFilter filter)
		{
			if (string.IsNullOrEmpty(filter.Keyword))
			{
				return null;
			}

			var result = await _playerHelper.SearchAsync(filter);

			return Json(result);
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

		[HttpGet]
		[Route("GetJingles")]
		public IActionResult GetJingles()
		{
			var result = JingleHelper.GetJingles();

			return Json(result);
		}

		[HttpPost]
		[Route("PlayJingle")]
		public IActionResult PlayJingle([FromBody] string filePath)
		{
			JingleHelper.Play(filePath);

			return Ok();
		}
	}
}
