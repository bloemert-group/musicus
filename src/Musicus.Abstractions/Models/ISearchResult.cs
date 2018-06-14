namespace Musicus.Abstractions.Models
{
	public interface ISearchResult
	{
		string TrackId { get; set; }
		string Artist { get; set; }
		string Description { get; set; }

		int TrackLength { get; set; }

		SearchResultType Type { get; set; }

		int TrackCount { get; set; }
		string Url { get; set; }
		TrackSource TrackSource { get; set; }
	}
}
