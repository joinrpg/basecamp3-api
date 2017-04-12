using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tsarev.Basecamp3.ApiProxy.Internal
{
  internal class BasecampAuthenticatorImpl : IBasecampAuthenticator
  {
    private BasecampCredentials credentials;
    private BasecampHostnameOptions hostname;
    private HttpClient _httpClient = new HttpClient();

    public BasecampAuthenticatorImpl(BasecampCredentials credentials, BasecampHostnameOptions hostname)
    {
      this.credentials = credentials;
      this.hostname = hostname;
    }

    public Uri GetRedirectUri()
    {
      var uriBuilder = new UriBuilder($"{hostname.LaunchpadHostname}/authorization/new");
      var parameters = HttpUtility.ParseQueryString("");
      parameters["type"] = "web_server";
      parameters["client_id"] = credentials.ClientId;
      parameters["redirect_uri"] = credentials.RedirectUri.AbsoluteUri;
      uriBuilder.Query = parameters.ToString();
      return uriBuilder.Uri;
    }

    public async Task<BasecampAuthenticationResult> Verify(string code)
    {
      var uri = new Uri($"{hostname.LaunchpadHostname}/authorization/token?type=web_server&client_id={credentials.ClientId}&redirect_uri={credentials.RedirectUri}&client_secret={credentials.ClientSecret}&code={code}");
      HttpResponseMessage tokenResponse = await _httpClient.PostAsync(uri, new StringContent(""));
      tokenResponse.EnsureSuccessStatusCode();
      string text = await tokenResponse.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<TokenVerificationResult>(text);
      return new BasecampAuthenticationResult {
        Success = true,
        Token = result.AsToken()
      };
    }

    private class TokenVerificationResult
    {
      public string access_token { get; set; }
      public int expires_in { get; set; }

      public string refresh_token { get; set; }

      public BasecampAuthenticationToken AsToken()
      {
        return new BasecampAuthenticationToken
        {
          AccessToken = access_token,
          RefreshToken = refresh_token,
          ValidUntil = DateTimeOffset.UtcNow.AddSeconds(expires_in)
        };
      }
    }
  }

  
}
