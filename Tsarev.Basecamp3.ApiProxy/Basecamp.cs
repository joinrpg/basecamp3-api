using System;
using System.Threading.Tasks;
using Tsarev.Basecamp3.ApiProxy.Internal;

namespace Tsarev.Basecamp3.ApiProxy
{
  public static class Basecamp 
  {
    public static IBasecampAuthenticator CreateAuthenticator(BasecampCredentials credentials, BasecampHostnameOptions hostname = null)
    {
      hostname = hostname ?? new BasecampHostnameOptions();
      return new BasecampAuthenticatorImpl(credentials, hostname);
    }

    public static IBasecampClient CreateClient(BasecampCredentials credentials, IBasecampTokenStorage tokenStorage, BasecampHostnameOptions hostname = null)
    {
      hostname = hostname ?? new BasecampHostnameOptions();
      return new BasecampClientImpl(credentials, tokenStorage, hostname);
    }

    /// <summary>
    /// Create Basecamp client. Note: if token will require refresh, this method will not save refreshed token to permanent storage.
    /// </summary>
    public static IBasecampClient CreateClient(BasecampCredentials credentials, BasecampAuthenticationToken token, BasecampHostnameOptions hostname = null)
    {
      return CreateClient(credentials, new BasecampTokenFakeStorage(token), hostname);
    }

    private class BasecampTokenFakeStorage : IBasecampTokenStorage
    {
      private BasecampAuthenticationToken token;

      public BasecampTokenFakeStorage(BasecampAuthenticationToken token)
      {
        this.token = token;
      }

      public BasecampAuthenticationToken GetToken() => token;

      public Task SaveToken(BasecampAuthenticationToken token)
      {
        this.token = token;
        return Task.FromResult(0);
      }  
    }
  }
}
