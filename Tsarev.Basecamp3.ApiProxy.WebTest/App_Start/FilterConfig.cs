using System.Web;
using System.Web.Mvc;

namespace Tsarev.Basecamp3.ApiProxy.WebTest
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}
