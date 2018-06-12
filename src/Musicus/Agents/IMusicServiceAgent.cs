using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Models;

namespace Musicus.Agents
{
	public interface IMusicServiceAgent
	{
		Task<bool> PlayAsync();
		Task<bool> PlayAsync(string url);
		Task<bool> PauseAsync();
		Task<bool> NextAsync(string url);
		Task<IList<SearchResult>> SearchAsync(string keyword);
		Task<MusicServiceStatus> GetStatusAsync();
		Task SetVolumeAsync(int volume);
		Task<int> GetVolumeAsync();
	}
}
