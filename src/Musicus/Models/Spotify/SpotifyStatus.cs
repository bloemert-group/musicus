using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musicus.Models.Spotify
{
	public class SpotifyStatus
	{
		public string Artist { get; set; }
		public string Track { get; set; }
		public double Current { get; set; }
		public double Length { get; set; }
		public string AlbumArtWork { get; set; }

		public bool IsPlaying { get; set; }
	}
}
