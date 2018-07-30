using Microsoft.Extensions.DependencyInjection;
using Musicus.Abstractions.Services;
using SpotifyService;

namespace Musicus.SpotifyService
{
	public static class SpotifyServiceConfiguration
	{
		public static IServiceCollection AddSpotifyMusicService(this IServiceCollection services, string spotifyClientId, string spotifyClientSecret)
		{
			if (!string.IsNullOrEmpty(spotifyClientSecret) && !string.IsNullOrEmpty(spotifyClientSecret))
			{
				services.AddTransient<IMusicService>(s => new SpotifyMusicService(spotifyClientId, spotifyClientSecret));
			}

			return services;
		}
	}
}
