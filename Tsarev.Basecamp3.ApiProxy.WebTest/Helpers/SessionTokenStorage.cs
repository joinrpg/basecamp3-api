using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Tsarev.Basecamp3.ApiProxy.WebTest.Helpers
{
  public class SessionTokenStorage : IBasecampTokenStorage
  {
    private HttpSessionStateBase session;

    public SessionTokenStorage(HttpSessionStateBase session)
    {
      this.session = session;
    }

    public BasecampAuthenticationToken GetToken()
    {
      return (BasecampAuthenticationToken)session["Token"];
    }

    public Task SaveToken(BasecampAuthenticationToken token)
    {
      session["Token"] = token;
      return Task.FromResult(0);
    }
  }
}