using Musicus.Abstractions.Models;

namespace Musicus.Models
{
	public class SearchResult : ISearchResult
	{
		public string TrackId { get; set; }
		public string Artist { get; set; }
		public string Description { get; set; }

		public int TrackLength { get; set; }

		public SearchResultType Type { get; set; }

		public int TrackCount { get; set; }
		public string Url { get; set; }
		public TrackSource TrackSource { get; set; }
		public string Icon { get; set; }
	}
}
