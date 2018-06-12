using Musicus.Abstractions.Models;

namespace Musicus.Models
{
	public class Track
	{
		public string TrackId { get; set; }
		public int TrackLength { get; set; }
		public string Artist { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		public TrackSource TrackSource { get; set; }
		public bool Played { get; set; }
	}
}
