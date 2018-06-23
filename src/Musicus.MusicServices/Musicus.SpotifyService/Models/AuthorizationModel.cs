using System;
using Newtonsoft.Json;

namespace Musicus.SpotifyService.Models
{
	public class AuthorizationModel
	{
		[JsonProperty(PropertyName = "access_token")]
		public string AccessToken { get; set; }
		[JsonProperty(PropertyName = "refresh_token")]
		public string RefreshToken { get; set; }

		private int _expiresIn;
		[JsonProperty(PropertyName = "expires_in")]
		public int ExpiresIn
		{
			get { return _expiresIn; }
			set
			{
				_expiresIn = value;

				ValidUntil = DateTime.Now.AddSeconds(value);
			}
		}

		[JsonProperty(PropertyName = "token_type")]
		public string TokenType { get; set; } = "Bearer";
		[JsonProperty(PropertyName = "scope")]
		public string Scope { get; set; } = "user-modify-playback-state";

		public DateTime ValidUntil { get; set; }

		public Boolean IsExpired() => ValidUntil <= DateTime.Now;
	}
}
