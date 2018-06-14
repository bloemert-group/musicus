using Musicus.Abstractions.Models;

namespace Musicus.Models
{
	public class SearchFilter
	{
		public string Keyword { get; set; }
		public TrackSource TrackSource { get; set; }
	}
}
