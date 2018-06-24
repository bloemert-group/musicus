﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Musicus.Abstractions.Models;
using Musicus.Models;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace Musicus.YouTubeService.Helpers
{
	public static class YouTubeHelper
	{
		private static YoutubeClient _youtubeService;
		public static YoutubeClient YouTubeService
			=> _youtubeService ?? (_youtubeService = new YoutubeClient());

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

		public static async Task<IActionResult<IList<ISearchResult>>> SearchAsync(string keyword)
		{
			var searchResult = await YouTubeService.SearchVideosAsync(keyword, 1);
			var result = new List<ISearchResult>();


			foreach (var item in searchResult)
			{
				result.Add(new SearchResult
				{
					Artist = item.Title,
					Description = string.Empty,
					Type = SearchResultType.Track,
					TrackSource = TrackSource.YouTube,
					TrackLength = int.Parse(item.Duration.TotalMilliseconds.ToString()),
					TrackId = item.Id,
					Url = item.Id,
				});
			}

			return ActionResult<IList<ISearchResult>>.Success(result);
		}

		public static IActionResult<object> Play()
		{
			if (VlcPlayer.GetMedia() != null)
			{
				VlcPlayer.Play();
				return ActionResult<object>.Success(_currentStatus);
			}
			return ActionResult<object>.Error("Unable to play");
		}

		public static async Task<IActionResult<object>> PlayAsync(string url)
		{
			try
			{
				var video = await YouTubeService.GetVideoMediaStreamInfosAsync(url);
				var audio = video.Audio.WithHighestBitrate();

				var t = new Thread(async () =>
				{
					var videoDetails = await YouTubeService.GetVideoAsync(url);

					_currentStatus = new MusicServiceStatus
					{
						Track = videoDetails?.Title,
						AlbumArtWork = videoDetails?.Thumbnails?.LowResUrl,
						TrackSource = TrackSource.YouTube,
						Artist = string.Empty
					};
				});
				t.Start();

				VlcPlayer.SetMedia(new Uri(audio.Url));

				VlcPlayer.Play();

				return ActionResult<object>.Success(_currentStatus);
			}
			catch (Exception ex)
			{
				return ActionResult<object>.Error(ex.Message);
			}
		}

		public static IActionResult<object> Pause()
		{
			VlcPlayer.Pause();

			return ActionResult<object>.Success(_currentStatus);
		}

		public static IActionResult<IMusicServiceStatus> GetStatus()
		{
			if (_currentStatus == null) return null;

			_currentStatus.IsPlaying = VlcPlayer.IsPlaying();
			_currentStatus.Current = VlcPlayer.Time / 1000;
			_currentStatus.Length = VlcPlayer.Length / 1000;

			return ActionResult<IMusicServiceStatus>.Success(_currentStatus);
		}

		public static IActionResult<float> GetVolume() => ActionResult<float>.Success(VlcPlayer.Audio.Volume);

		public static IActionResult<float> SetVolume(float volume)
		{
			VlcPlayer.Audio.Volume = (int)volume;

			return ActionResult<float>.Success(VlcPlayer.Audio.Volume);
		}
	}
}
