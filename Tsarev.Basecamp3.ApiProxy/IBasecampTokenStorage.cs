using System.Threading.Tasks;

namespace Tsarev.Basecamp3.ApiProxy
{
  public interface IBasecampTokenStorage
  {
    BasecampAuthenticationToken GetToken();
    Task SaveToken(BasecampAuthenticationToken token);
  }
}
