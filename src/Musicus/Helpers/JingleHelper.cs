using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Musicus.Models;

namespace Musicus.Helpers
{
	public static class JingleHelper
	{
		public static string JingleFilePath { get; set; }

		private static bool _jinglePlaying;

		private static Vlc.DotNet.Core.VlcMediaPlayer _vlcPlayer;
		public static Vlc.DotNet.Core.VlcMediaPlayer VlcPlayer
		{
			get
			{
				if (_vlcPlayer == null)
				{
					var libDirectory = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

					_vlcPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(libDirectory, new string[] { "--no-one-instance" });

					_vlcPlayer.EndReached += _vlcPlayer_EndReached;
				}
				return _vlcPlayer;
			}
		}

		private static void _vlcPlayer_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
		{
			_jinglePlaying = false;
		}

		public static void InitVolume(float volume)
		{
			VlcPlayer.Audio.Volume = 200;
		}

		public static IEnumerable<Jingle> GetJingles()
		{
			if (string.IsNullOrEmpty(JingleFilePath) || !Directory.Exists(JingleFilePath))
			{
				throw new ArgumentException(nameof(JingleFilePath), "No (valid) path was given for jingles");
			}

			var dirInfo = new DirectoryInfo(JingleFilePath);

			var files = dirInfo.GetFiles("*.*");

			return files.Select(file => new Jingle { Name = file.Name, FilePath = file.FullName });
		}

		public static async Task PlayAsync(string filePath)
		{
			_jinglePlaying = true;

			VlcPlayer.SetMedia(new FileInfo(filePath));

			VlcPlayer.Play();

			while (_jinglePlaying)
			{
				await Task.Delay(500);
			}
		}
	}
}
