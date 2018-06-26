using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Musicus.Abstractions.Services;
using SpotifyService;

namespace Musicus.SpotifyService
{
	public static class SpotifyServiceConfiguration
	{
		public static IServiceCollection AddSpotifyMusicService(this IServiceCollection services, IConfiguration config)
		{
			var spotifyClientId = config["SpotifyClientId"];
			var spotifyClientSecret = config["SpotifyClientSecret"];

			if (!string.IsNullOrEmpty(spotifyClientSecret) && !string.IsNullOrEmpty(spotifyClientSecret))
			{
				services.AddTransient<IMusicService>(s => new SpotifyMusicService(spotifyClientId, spotifyClientSecret));
			}

			return services;
		}
	}
}
