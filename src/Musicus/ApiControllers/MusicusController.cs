using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		[Route("play/{spotifyUrl}")]
		public IActionResult Play(string spotifyUrl)
		{
			return Json(SpotifyHelper.Play(spotifyUrl));
		}

		[Route("status")]
		public IActionResult Status()
		{
			return Json(SpotifyHelper.GetStatus());
		}

		[Route("next")]
		public IActionResult Next()
		{
			SpotifyHelper.Next();

			return Json(new { Succeed = true });
		}		

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
	}
}
