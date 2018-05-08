using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Musicus.Helpers
{
	public static class JingleHelper
	{
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
