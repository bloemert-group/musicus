using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Abstractions.Services;

namespace YouTubeService
{
	public class YouTubeMusicService : IMusicService
	{
		public TrackSource TrackSource => TrackSource.YouTube;

		public Task<IMusicServiceStatus> GetStatusAsync()
		{
			throw new NotImplementedException();
		}

		public Task<bool> NextAsync(string url)
		{
			throw new NotImplementedException();
		}

		public Task<bool> PauseAsync()
		{
			throw new NotImplementedException();
		}

		public Task<bool> PlayAsync()
		{
			throw new NotImplementedException();
		}

		public Task<bool> PlayAsync(string url)
		{
			throw new NotImplementedException();
		}

		public async Task<IList<ISearchResult>> SearchAsync(string keyword)
		{
			return new List<ISearchResult>();
		}
	}
}
