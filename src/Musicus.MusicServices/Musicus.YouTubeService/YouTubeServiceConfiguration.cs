using Microsoft.Extensions.DependencyInjection;
using Musicus.Abstractions.Services;
using YouTubeService;

namespace Musicus.YouTubeService
{
	public static class YouTubeServiceConfiguration
	{
		public static IServiceCollection AddYouTubeMusicService(this IServiceCollection services)
		{
			services.AddTransient<IMusicService, YouTubeMusicService>();

			return services;
		}
	}
}
