namespace Musicus.Models
{
	public enum SearchResultType
	{
		Track, Playlist
	}

	public class SearchResult
	{
		public string TrackId { get; set; }
		public string Description { get; set; }

		public int TrackLength { get; set; }

		public SearchResultType Type { get; set; }

		public int TrackCount { get; set; }
		public string Url { get; set; }
		public string TrackSource { get; set; }
	}
}
