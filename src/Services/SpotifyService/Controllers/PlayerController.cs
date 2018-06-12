using Microsoft.AspNetCore.Mvc;
using Musicus.Helpers;

namespace SpotifyService.Controllers
{
	[Route("api/player")]
	public class PlayerController : Controller
	{
		[HttpPost]
		[Route("nexttrack")]
		public IActionResult NextTrack(string url)
		{
			SpotifyHelper.Next(url);

			return Json(new { Succeed = true });
		}

		[HttpGet]
		[Route("pause")]
		public IActionResult Pause()
		{
			SpotifyHelper.Pause();

			return Json(new { Succeed = true });
		}

		[HttpGet]
		[Route("play")]
		public IActionResult Play() => Json(new { Succeed = SpotifyHelper.Play() });

		[HttpPost]
		[Route("play")]
		public IActionResult Play([FromBody]string url) => Json(new { Succeed = SpotifyHelper.Play(url) });

		[HttpPost]
		[Route("search")]
		public IActionResult Search(string keyword) => Json(new { Succeed = true, Data = SpotifyHelper.Search(keyword) });

		[HttpPost]
		[Route("setvolume")]
		public IActionResult SetVolume([FromBody] int volume)
		{
			SpotifyHelper.SetVolume(volume);

			return Ok();
		}

		[HttpGet]
		[Route("getvolume")]
		public IActionResult GetVolume() => Json(SpotifyHelper.GetVolume());
	}
}