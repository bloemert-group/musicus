using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Musicus.FileSystemService;
using Musicus.Helpers;
using Musicus.SpotifyService;
using Musicus.YouTubeService;

namespace Musicus
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<Player>();

			services.AddMvc();
			services.AddSignalR();

			SetMusicServices(services);

			services.AddSingleton<SignalRHelper>();

			return services.BuildServiceProvider();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, SignalRHelper signalRHelper)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseCors("AllowSpecificOrigin");

			app.UseSignalR(routes =>
			{
				routes.MapHub<MusicusHub>("/musicushub");
			});

			app.UseMvcWithDefaultRoute();

			if (signalRHelper != null)
			{
				Task.Run(() => signalRHelper.StartStatusUpdate()).ConfigureAwait(false);

				Player.ExceptionHandler = signalRHelper.ShowError;
			}

			JingleHelper.JingleFilePath = Configuration[nameof(JingleHelper.JingleFilePath)];
			Player.DefaultMusicServiceVolumeLevel = int.TryParse(Configuration[nameof(Player.DefaultMusicServiceVolumeLevel)], out var volume) ? volume : 30;
		}

		private void SetMusicServices(IServiceCollection services)
		{
			services.AddFileSystemMusicService(Configuration["FileSystemMusicServiceFilePath"]);
			services.AddSpotifyMusicService(Configuration["SpotifyClientId"], Configuration["SpotifyClientSecret"]);
			services.AddYouTubeMusicService();
		}
	}
}
