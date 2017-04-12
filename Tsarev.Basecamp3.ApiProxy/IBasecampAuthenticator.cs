using System;
using System.Threading.Tasks;

namespace Tsarev.Basecamp3.ApiProxy
{
  public interface IBasecampAuthenticator
  {
    Uri GetRedirectUri();
    Task<BasecampAuthenticationResult> Verify(string code);
  }

  public class BasecampAuthenticationResult
  {
    public bool Success { get; set; }
    public BasecampAuthenticationToken Token { get; set; }
  }

}
