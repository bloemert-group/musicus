using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Musicus.Helpers;
using Musicus.Models;

namespace Musicus.ApiControllers
{
	[Route("api/musicus")]
	public class MusicusController : Controller
	{
		private SignalRHelper _signalRHelper;
		private Player _player;

		public MusicusController(SignalRHelper signalRHelper, Player player)
		{
			_signalRHelper = signalRHelper;
			_player = player;
		}

		[HttpPost]
		[Route("play")]
		public async Task<IActionResult> PlayAsync([FromBody]Track track)
		{
			var result = await _player.PlayAsync(track);

			return Json(new { Succeed = result });
		}

		[HttpPost]
		[Route("pause")]
		public async Task<IActionResult> PauseAsync([FromBody]Track track)
		{
			var result = await _player.PauseTrackAsync(track);

			return Json(new { Succeed = result });
		}

		[HttpGet]
		[Route("status/{tracksource}")]
		public async Task<IActionResult> StatusAsync()
		{
			var currentTrack = Playlist.GetCurrentTrack();
			if (currentTrack == null)
			{
				return Ok();
			}

			var result = await _player.GetStatusAsync(currentTrack);

			return Json(result);
		}

		[HttpPost]
		[Route("next")]
		public async Task<IActionResult> NextAsync()
		{
			var result = await _player.PlayNextTrackAsync();

			return Json(result);
		}

		[HttpPost]
		[Route("setvolume")]
		public async Task<IActionResult> SetVolumeAsync([FromBody] VolumeFilter volumeFilter)
		{
			await _player.SetVolumeAsync(volumeFilter);

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

			var result = await _player.SearchAsync(filter);

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
		[Route("getjingles")]
		public IActionResult GetJingles()
		{
			var result = JingleHelper.GetJingles();

			return Json(result);
		}

		[HttpPost]
		[Route("playjingle")]
		public async Task<IActionResult> PlayJingleAsync([FromBody] string filePath)
		{
			const int reduceVolume = 15;

			var currentTrack = Playlist.GetCurrentTrack();
			if (currentTrack != null)
			{
				await SetVolumeAsync(new VolumeFilter { TrackSource = currentTrack.TrackSource, Volume = Player.CurrentVolume - reduceVolume });
			}

			JingleHelper.Play(filePath);

			if (currentTrack != null)
			{
				await SetVolumeAsync(new VolumeFilter { TrackSource = currentTrack.TrackSource, Volume = Player.CurrentVolume });
			}

			return Ok();
		}
	}
}
