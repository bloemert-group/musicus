using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Musicus.Models;
using Newtonsoft.Json;

namespace Musicus.Agents
{
	public class JingleAgent : IJingleAgent
	{
		private string _jingleServiceUrl { get; set; }

		public JingleAgent(string jingleServiceUrl)
		{
			_jingleServiceUrl = jingleServiceUrl;
		}

		private HttpClient _httpClient;
		protected HttpClient HttpClient
		{
			get
			{
				if (string.IsNullOrEmpty(_jingleServiceUrl))
				{
					throw new ArgumentException(nameof(_jingleServiceUrl), "No url for jingle service was given");
				}

				if (_httpClient == null)
				{
					_httpClient = new HttpClient
					{
						BaseAddress = new Uri(_jingleServiceUrl)
					};
					_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				}
				return _httpClient;
			}
		}

		public async Task<IEnumerable<Jingle>> GetJinglesAsync()
		{
			var result = await HttpClient.GetAsync("api/jingle");

			var jsonResult = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<IEnumerable<Jingle>>(jsonResult);
		}

		public async Task PlayAsync(string filePath)
		{
			var result = await HttpClient.PostAsync("api/jingle/play", new StringContent(filePath));
		}
	}
}
