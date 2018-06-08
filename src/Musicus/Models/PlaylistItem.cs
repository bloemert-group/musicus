namespace Musicus.Models
{
	public class PlaylistItem
	{
		public string TrackId { get; set; }
		public int TrackLength { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		public string TrackSource { get; set; }
		public bool Played { get; set; }
	}
}
