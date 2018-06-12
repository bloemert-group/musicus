using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Models;

namespace Musicus.Agents
{
	public interface IJingleAgent
	{
		Task<IEnumerable<Jingle>> GetJinglesAsync();
		Task PlayAsync(string filePath);
	}
}
