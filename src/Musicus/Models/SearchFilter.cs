using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musicus.Models
{
	public class SearchFilter
	{
		public string Keyword { get; set; }
		public bool Spotify { get; set; }
		public bool Youtube { get; set; }
	}
}
