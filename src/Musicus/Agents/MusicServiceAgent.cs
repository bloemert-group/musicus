using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Musicus.Models;
using Newtonsoft.Json;

namespace Musicus.Agents
{
	/// <summary>
	/// TODO: Check statuscode after request is made
	/// </summary>
	public class MusicServiceAgent : IMusicServiceAgent
	{
		const string NextTrackUrl = "nexttrack";
		const string PlayUrl = "play";
		const string PauseUrl = "pause";
		const string SearchUrl = "search";
		const string GetStatusUrl = "getstatus";
		const string SetVolumeUrl = "setvolume";
		const string GetVolumeUrl = "getvolume";

		private string _musicServiceUrl;

		public MusicServiceAgent(string musicServiceUrl)
		{
			_musicServiceUrl = musicServiceUrl;
		}

		private HttpClient _httpClient;
		protected HttpClient HttpClient
		{
			get
			{
				if (_httpClient == null)
				{
					_httpClient = new HttpClient
					{
						BaseAddress = new Uri($"{_musicServiceUrl}/api/player")
					};
					_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				}
				return _httpClient;
			}
		}

		public async Task<bool> NextAsync(string url)
		{
			var result = await HttpClient.PostAsync(NextTrackUrl, new StringContent(url));

			var resultJson = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<bool>(resultJson);
		}

		public async Task<bool> PauseAsync()
		{
			var result = await HttpClient.GetAsync(PauseUrl);

			var resultJson = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<bool>(resultJson);
		}

		public async Task<bool> PlayAsync()
		{
			var result = await HttpClient.GetAsync(PlayUrl);

			var resultJson = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<bool>(resultJson);
		}

		public async Task<bool> PlayAsync(string url)
		{
			var result = await HttpClient.PostAsync(PlayUrl, new StringContent(url));

			var resultJson = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<bool>(resultJson);
		}

		public async Task<IList<SearchResult>> SearchAsync(string keyword)
		{
			var result = await HttpClient.PostAsync(SearchUrl, new StringContent(keyword));

			var resultJson = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<IList<SearchResult>>(resultJson);
		}

		public async Task<MusicServiceStatus> GetStatusAsync()
		{
			var result = await HttpClient.GetAsync(GetStatusUrl);

			var resultJson = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<MusicServiceStatus>(resultJson);
		}

		public async Task SetVolumeAsync(int volume)
		{
			var result = await HttpClient.PostAsync(SetVolumeUrl, new StringContent(volume.ToString()));
		}

		public async Task<int> GetVolumeAsync()
		{
			var result = await HttpClient.GetAsync(GetVolumeUrl);

			var resultJson = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<int>(resultJson);
		}
	}
}
