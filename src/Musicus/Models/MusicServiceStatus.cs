namespace Musicus.Models
{
	public class MusicServiceStatus
	{
		public string Artist { get; set; }
		public string Track { get; set; }
		public double Current { get; set; }
		public double Length { get; set; }
		public string AlbumArtWork { get; set; }

		public bool IsPlaying { get; set; }
		public string TrackSource { get; set; }
	}
}
