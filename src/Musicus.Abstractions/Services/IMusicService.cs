using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;

namespace Musicus.Abstractions.Services
{
	public delegate void TrackEndHandler();

	public interface IMusicService
	{
		TrackSource TrackSource { get; }

		Task<IActionResult<object>> PlayAsync();
		Task<IActionResult<object>> PlayAsync(string url);
		Task<IActionResult<object>> PauseAsync();
		Task<IActionResult<object>> NextAsync(string url);
		Task<IActionResult<IList<ISearchResult>>> SearchAsync(string keyword);
		Task<IActionResult<IMusicServiceStatus>> GetStatusAsync();
		Task<IActionResult<float>> SetVolumeAsync(float volume);
		Task<IActionResult<float>> GetVolumeAsync();

		event TrackEndHandler TrackEndedEvent;
		void OnTrackEnded();
	}
}
