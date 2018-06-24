using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Services;
using Musicus.Abstractions.Models;
using Musicus.Models;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace Musicus.YouTubeService.Helpers
{
	public static class YouTubeHelper
	{
		public static string ApiKey { get; set; }

		private static Google.Apis.YouTube.v3.YouTubeService _youtubeService;
		public static Google.Apis.YouTube.v3.YouTubeService YouTubeService
		{
			get
			{

				if (_youtubeService == null)
				{
					_youtubeService = new Google.Apis.YouTube.v3.YouTubeService(new BaseClientService.Initializer()
					{
						ApiKey = ApiKey,
						ApplicationName = "Musicus.YouTubeService"
					});
				}
				return _youtubeService;
			}
		}

		private static Vlc.DotNet.Core.VlcMediaPlayer _vlcPlayer;
		public static Vlc.DotNet.Core.VlcMediaPlayer VlcPlayer
		{
			get
			{
				if (_vlcPlayer == null)
				{
					var libDirectory = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

					_vlcPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(libDirectory);
					_vlcPlayer.EndReached += _vlcPlayer_EndReached;
				}
				return _vlcPlayer;
			}
		}

		private static void _vlcPlayer_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
		{
			_currentStatus = null;
			VlcPlayer.Stop();
		}

		private static IMusicServiceStatus _currentStatus;

		public static async Task<IList<ISearchResult>> SearchAsync(string keyword)
		{
			var request = YouTubeService.Search.List("snippet");
			request.MaxResults = 20;
			request.Type = "video";
			request.Q = keyword;
			var result = new List<ISearchResult>();

			var searchResult = await request.ExecuteAsync();

			foreach (var item in searchResult.Items)
			{
				var videoDetailRequest = YouTubeService.Videos.List("contentDetails");
				videoDetailRequest.Id = item.Id.VideoId;

				var videoContentDetail = await videoDetailRequest.ExecuteAsync();

				var reg = new Regex("([0-9]*)M([0-9]*)S$");
				var regMatch = reg.Match(videoContentDetail.Items[0]?.ContentDetails.Duration);

				var duration = 0;
				if (regMatch != null && regMatch.Groups.Count > 2)
				{
					var minutes = int.Parse(regMatch.Groups[1].Value);
					var seconds = int.Parse(regMatch.Groups[2].Value);
					duration = (minutes * 60) + seconds;
				}

				result.Add(new SearchResult
				{
					Artist = item.Snippet.Title,
					Description = string.Empty,
					Type = SearchResultType.Track,
					TrackSource = TrackSource.YouTube,
					TrackLength = duration * 1000,
					TrackId = item.Id.VideoId,
					Url = item.Id.VideoId,
					Icon = "youtube icon"
				});
			}

			return result;
		}

		public static bool Play()
		{
			if (VlcPlayer.GetMedia() != null)
			{
				VlcPlayer.Play();
				return true;
			}
			return false;
		}

		public static async Task<bool> PlayAsync(string url)
		{
			try
			{
				var yt = new YoutubeClient();

				var video = await yt.GetVideoMediaStreamInfosAsync(url);
				var audio = video.Audio.WithHighestBitrate();

				var t = new Thread(async () =>
				{
					var videoDetailRequest = YouTubeService.Videos.List("snippet");
					videoDetailRequest.Id = url;

					var videoContentDetail = await videoDetailRequest.ExecuteAsync();
					var track = videoContentDetail.Items[0];

					_currentStatus = new MusicServiceStatus
					{
						Track = track.Snippet.Title,
						AlbumArtWork = track.Snippet.Thumbnails.Standard.Url,
						TrackSource = TrackSource.YouTube,
						Artist = string.Empty
					};
				});
				t.Start();

				VlcPlayer.SetMedia(new Uri(audio.Url));

				VlcPlayer.Play();

				return true;
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public static bool Pause()
		{
			VlcPlayer.Pause();

			return true;
		}

		public static IMusicServiceStatus GetStatus()
		{
			if (_currentStatus == null) return null;

			_currentStatus.IsPlaying = VlcPlayer.IsPlaying();
			_currentStatus.Current = VlcPlayer.Time / 1000;
			_currentStatus.Length = VlcPlayer.Length / 1000;

			return _currentStatus;
		}

		public static float GetVolume() => VlcPlayer.Audio.Volume;

		public static bool SetVolume(float volume)
		{
			VlcPlayer.Audio.Volume = (int)volume;

			return true;
		}
	}
}
