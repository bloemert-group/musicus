using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;

namespace Musicus.Abstractions.Services
{
	public interface IMusicService
	{
		TrackSource TrackSource { get; }

		Task<bool> PlayAsync();
		Task<bool> PlayAsync(string url);
		Task<bool> PauseAsync();
		Task<bool> NextAsync(string url);
		Task<IList<ISearchResult>> SearchAsync(string keyword);
		Task<IMusicServiceStatus> GetStatusAsync();
		Task SetVolumeAsync(int volume);
		Task<float> GetVolumeAsync();
	}
}
