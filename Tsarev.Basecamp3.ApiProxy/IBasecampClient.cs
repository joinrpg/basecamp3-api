using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tsarev.Basecamp3.ApiProxy
{
  public interface IBasecampClient
  {
    Task<IReadOnlyCollection<BasecampAccount>> GetAccounts();
    Task<TResult> Get<TResult>(string uri);
  }
}
