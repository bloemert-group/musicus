using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Musicus.JingleService.Models;

namespace Musicus.JingleService.Helpers
{
	public static class JingleHelper
	{
		public static string JingleFilePath { get; set; }

		public static IEnumerable<Jingle> GetJingles()
		{
			if (string.IsNullOrEmpty(JingleFilePath) || !Directory.Exists(JingleFilePath))
			{
				throw new ArgumentException(nameof(JingleFilePath), "No (valid) path was given for jingles");
			}

			var dirInfo = new DirectoryInfo(JingleFilePath);

			var files = dirInfo.GetFiles("*.*");

			return files.Select(file => new Jingle { Name = file.Name, Path = file.FullName });
		}

		public static void Play(string filePath)
		{
			var libDirectory = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

			var mediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(libDirectory);
			mediaPlayer.SetMedia(new FileInfo(filePath));

			bool playFinished = false;
			mediaPlayer.EncounteredError += (sender, e) =>
			{
				playFinished = true;
			};

			mediaPlayer.EndReached += (sender, e) =>
			{
				playFinished = true;
			};

			mediaPlayer.Play();

			while (!playFinished)
			{
				Thread.Sleep(TimeSpan.FromMilliseconds(500));
			}
		}
	}
}
