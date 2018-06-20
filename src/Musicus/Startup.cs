using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Musicus.Abstractions.Services;
using Musicus.Helpers;
using SpotifyService;
using YouTubeService;

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
			services.AddSingleton<SignalRHelper>();
			services.AddSingleton<Player>();

			services.AddMvc();
			services.AddSignalR();

			if (Configuration["SpotifyClientId"] != String.Empty && Configuration["SpotifyClientSecret"] != String.Empty)
			{
				services.AddTransient<IMusicService>(s => new SpotifyMusicService(Configuration["SpotifyClientId"], Configuration["SpotifyClientSecret"]));
			}

			if (Configuration["YouTubeApiKey"] != String.Empty)
			{
				services.AddTransient<IMusicService>(s => new YouTubeMusicService(Configuration["YouTubeApiKey"]));
			}

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
				signalRHelper.StartStatusUpdate();
			}

			JingleHelper.JingleFilePath = Configuration[nameof(JingleHelper.JingleFilePath)];
		}
	}
}
