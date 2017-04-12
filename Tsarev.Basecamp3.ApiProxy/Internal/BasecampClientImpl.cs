using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Tsarev.Basecamp3.ApiProxy.Internal
{
  public class BasecampClientImpl : IBasecampClient
  {
    private BasecampCredentials credentials;
    private BasecampHostnameOptions hostname;
    private IBasecampTokenStorage tokenStorage;
    private BasecampAuthenticationToken token;
    private HttpClient _httpClient = new HttpClient();

    public BasecampClientImpl(BasecampCredentials credentials, IBasecampTokenStorage tokenStorage, BasecampHostnameOptions hostname)
    {
      this.credentials = credentials;
      this.tokenStorage = tokenStorage;
      this.hostname = hostname;
      token = this.tokenStorage.GetToken();
    }

    private class BasecampIdentity
    {
      public int id { get; set; }
      public string first_name { get; set; }
      public string last_name { get; set; }
      public string email_address { get; set; }
    }


    private class BasecampAuthorizationResult
    {
      public DateTimeOffset expires_at { get; set; }
      public BasecampIdentity identity { get; set; }
      public List<BasecampAccountResult> accounts { get; set; }
    }

    public async Task<IReadOnlyCollection<BasecampAccount>> GetAccounts()
    {
      var result = await GetLaunchpad<BasecampAuthorizationResult>("authorization");
      return result.accounts.Where(acc => acc.product == "bc3").Select(acc => new BasecampAccount(acc, this)).ToList();
    }

    private Task<TResult> GetLaunchpad<TResult>(string apiName)
    {
      return Get<TResult>($"{hostname.LaunchpadHostname}/{apiName}.json");
    }

    public async Task<TResult> Get<TResult>(string apiUrl)
    {
      await RefreshTokenIfRequired();
      var text = await ExecuteRequest(apiUrl);
      return JsonConvert.DeserializeObject<TResult>(text);
    }

    private class TokenRefreshResult
    {
      public string access_token { get; set; }
      public int expires_in { get; set; }
    }

    private async Task<string> ExecuteRequest(string apiUrl)
    {
      var message = new HttpRequestMessage(HttpMethod.Get, apiUrl);
      message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
      HttpResponseMessage tokenResponse = await _httpClient.SendAsync(message);
      tokenResponse.EnsureSuccessStatusCode();
      return await tokenResponse.Content.ReadAsStringAsync();
    }

    private async Task RefreshTokenIfRequired()
    {
      if ((token.ValidUntil - DateTimeOffset.UtcNow).TotalDays < 2)
      {
        await RefreshToken();
      }
    }

    private async Task RefreshToken()
    {
      var uri = new Uri($"{hostname.LaunchpadHostname}/authorization/token?type=refresh&client_id={credentials.ClientId}&redirect_uri={credentials.RedirectUri}&client_secret={credentials.ClientSecret}&refresh_token={token.RefreshToken}");
      HttpResponseMessage tokenResponse = await _httpClient.PostAsync(uri, new StringContent(""));
      tokenResponse.EnsureSuccessStatusCode();
      string text = await tokenResponse.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<TokenRefreshResult>(text);
      token.AccessToken = result.access_token;
      token.ValidUntil = DateTimeOffset.UtcNow.AddSeconds(result.expires_in);
      await tokenStorage.SaveToken(token);
    }
  }
}
