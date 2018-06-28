using System;
using Microsoft.Extensions.DependencyInjection;
using Musicus.Abstractions.Services;

namespace Musicus.FileSystemService
{
	public static class FileSystemServiceConfiguration
	{
		public static IServiceCollection AddFileSystemMusicService(this IServiceCollection services, string filePath)
		{
			if (!string.IsNullOrEmpty(filePath))
			{
				services.AddTransient<IMusicService>(s => new FileSystemMusicService(filePath));
			}
			else
			{
				throw new ArgumentException("ClientId and/or ClientSecret are not provided! If you're not in possession of the Spotify credentials, consider disabling this service");
			}
			return services;
		}
	}
}
