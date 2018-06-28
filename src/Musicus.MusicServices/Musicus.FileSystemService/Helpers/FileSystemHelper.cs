using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FFmpeg.NET;
using Musicus.Abstractions.Models;
using Musicus.Models;

namespace Musicus.FileSystemService.Helpers
{
	public static class FileSystemHelper
	{
		private static DirectoryInfo _vlcLibDir =>
			new DirectoryInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

		public static string FilePath { get; set; }

		private static DirectoryInfo _directory;
		public static DirectoryInfo Directory
		{
			get
			{
				if (_directory == null)
				{
					_directory = new DirectoryInfo(FilePath);
					if (!_directory.Exists)
					{
						throw new ArgumentException($"The directory '{FilePath}' does not exist");
					}
				}
				return _directory;
			}
		}

		private static Vlc.DotNet.Core.VlcMediaPlayer _vlcPlayer;
		public static Vlc.DotNet.Core.VlcMediaPlayer VlcPlayer
		{
			get
			{
				if (_vlcPlayer == null)
				{
					_vlcPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(_vlcLibDir);
				}
				return _vlcPlayer;
			}
		}

		private static IMusicServiceStatus _currentStatus;

		public static IActionResult<IList<ISearchResult>> Search(string keyword)
		{
			var result = new List<ISearchResult>();

			var files = Directory.GetFiles($"*{keyword}*.mp3", SearchOption.AllDirectories);

			var vlcPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(_vlcLibDir);
			foreach (var file in files.Take(20))
			{
				var ffmpeg = new FFmpeg.NET.Engine.FFmpeg();
				var audioFile = ffmpeg.GetMetaData(new MediaFile(file.FullName));

				// We don't want to see the extension in the description
				var description = file.Name.Replace(file.Extension, "");

				result.Add(new SearchResult
				{
					Description = description,
					TrackId = file.Name,
					TrackLength = int.Parse(audioFile.Duration.TotalMilliseconds.ToString()),
					Url = file.FullName,
					TrackSource = TrackSource.FileSystem,
					Icon = "hdd icon"
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

		public static IActionResult<object> Play(string url)
		{
			var file = new FileInfo(url);

			VlcPlayer.SetMedia(file);

			var description = file.Name.Replace(file.Extension, "");

			_currentStatus = new MusicServiceStatus
			{
				Track = description,
				TrackSource = TrackSource.FileSystem,
				Artist = string.Empty
			};

			VlcPlayer.Play();

			return ActionResult<object>.Success(_currentStatus);
		}

		public static IActionResult<object> Pause()
		{
			try
			{
				VlcPlayer.Pause();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

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
