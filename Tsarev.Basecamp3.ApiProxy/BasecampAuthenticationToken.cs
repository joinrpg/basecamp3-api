using System;

namespace Tsarev.Basecamp3.ApiProxy
{
  public class BasecampAuthenticationToken
  {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset ValidUntil { get; set; }

    public override string ToString()
    {
      return $"BasecampAuthenticationToken(AccessToken: {AccessToken}, RefreshToken: {RefreshToken}, ValidUntil: {ValidUntil})";
    }
  }
}
