using System;
using System.Configuration;

namespace Tsarev.Basecamp3.ApiProxy.WebTest.Helpers
{
    internal static class ApiSecretsStorage 
    {
      internal static string BasecampClientId => ConfigurationManager.AppSettings[nameof(BasecampClientId)];

      internal static string BasecampClientSecret => ConfigurationManager.AppSettings[nameof(BasecampClientSecret)];

      internal static BasecampCredentials Credentials => new BasecampCredentials()
      {
        ClientId = ApiSecretsStorage.BasecampClientId,
        ClientSecret = ApiSecretsStorage.BasecampClientSecret,
        RedirectUri = new Uri("http://localhost:52368/home/bcauth")
      };
    }
}