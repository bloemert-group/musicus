namespace Musicus.Abstractions.Models
{
	public interface IMusicServiceStatus
	{
		string Artist { get; set; }
		string Track { get; set; }
		double Current { get; set; }
		double Length { get; set; }
		string AlbumArtWork { get; set; }

		bool IsPlaying { get; set; }
		string TrackSource { get; set; }
	}
}
