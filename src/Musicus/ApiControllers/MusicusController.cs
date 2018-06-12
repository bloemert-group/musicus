using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Musicus.Agents;
using Musicus.Helpers;
using Musicus.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Musicus.ApiControllers
{
	[Route("api/musicus")]
	public class MusicusController : Controller
	{
		private SignalRHelper _signalRHelper;
		private IJingleAgent _jingleAgent;

		public MusicusController(SignalRHelper signalRHelper)
		{
			_signalRHelper = signalRHelper;
			_jingleAgent = new JingleAgent("TODO");
		}

		[HttpPost]
		[Route("play")]
		public async Task<IActionResult> PlayAsync([FromBody]Track track)
		{
			var result = await PlayerHelper.PlayAsync(track);

			return Json(new { Succeed = result });
		}

		[HttpPost]
		[Route("pause")]
		public async Task<IActionResult> PauseAsync([FromBody]Track track)
		{
			var result = await PlayerHelper.PauseTrackAsync(track);

			return Json(new { Succeed = result });
		}

		[HttpGet]
		[Route("status/{tracksource}")]
		public async Task<IActionResult> StatusAsync(TrackSource trackSource)
		{
			var result = await PlayerHelper.GetStatusAsync(trackSource);

			return Json(result);
		}

		[HttpPost]
		[Route("next")]
		public async Task<IActionResult> NextAsync()
		{
			var result = await PlayerHelper.PlayNextTrackAsync();

			return Json(result);
		}

		[HttpPost]
		[Route("setvolume")]
		public async Task<IActionResult> SetVolumeAsync([FromBody] VolumeFilter volumeFilter)
		{
			await PlayerHelper.SetVolumeAsync(volumeFilter);

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

			var result = await PlayerHelper.SearchAsync(filter);

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
		public async Task<IActionResult> GetJinglesAsync()
		{
			var result = await _jingleAgent.GetJinglesAsync();

			return Json(result);
		}

		[HttpPost]
		[Route("PlayJingle")]
		public async Task<IActionResult> PlayJingleAsync([FromBody] string filePath)
		{
			await _jingleAgent.PlayAsync(filePath);

			return Ok();
		}
	}
}
