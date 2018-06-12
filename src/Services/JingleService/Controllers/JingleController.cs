using Microsoft.AspNetCore.Mvc;
using Musicus.JingleService.Helpers;

namespace JingleService.Controllers
{
	[Route("api/jingle")]
	public class JingleController : Controller
	{
		// GET api/values
		[HttpGet]
		public IActionResult Get() => Json(JingleHelper.GetJingles());

		// GET api/values/5
		[HttpPost("play")]
		public IActionResult Play([FromBody]string filePath)
		{
			JingleHelper.Play(filePath);

			return Ok();
		}

	}
}
