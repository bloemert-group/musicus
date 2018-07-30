using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Musicus.Abstractions.Services;

namespace Musicus.FileSystemService
{
	public static class FileSystemServiceConfiguration
	{
		public static IServiceCollection AddFileSystemMusicService(this IServiceCollection services, string filePath)
		{
			if (!File.Exists(filePath))
			{
				filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
				Console.WriteLine($"The provided filepath was not found, we are now using {filePath} as the music folder.");
			}

			if (!string.IsNullOrEmpty(filePath))
			{
				services.AddTransient<IMusicService>(s => new FileSystemMusicService(filePath));
			}
			else
			{
				throw new ArgumentException("The filesystem music service cannot find the specified and the Music folder on your machine.");
			}
			return services;
		}
	}
}
