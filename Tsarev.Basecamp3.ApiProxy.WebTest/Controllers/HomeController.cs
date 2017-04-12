using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tsarev.Basecamp3.ApiProxy.WebTest.Helpers;

namespace Tsarev.Basecamp3.ApiProxy.WebTest.Controllers
{
  public class HomeController : Controller
  {
    private IBasecampClient Client
    {
      get
      {
        var storage = new SessionTokenStorage(Session);
        return Basecamp.CreateClient(ApiSecretsStorage.Credentials, storage);
      }
    }
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult StartAuth()
    {
      var auth = Basecamp.CreateAuthenticator(ApiSecretsStorage.Credentials);
      return Redirect(auth.GetRedirectUri().AbsoluteUri);
    }

    public async Task<ActionResult> BcAuth(string code)
    {
      var auth = Basecamp.CreateAuthenticator(ApiSecretsStorage.Credentials);
      var result = await auth.Verify(code);
      var storage = new SessionTokenStorage(Session);
      await storage.SaveToken(result.Token);
      return RedirectToAction("Menu");
    }

    [HttpGet]
    public async Task<ActionResult> Menu()
    {
      ViewBag.Accounts = await Client.GetAccounts();
      return View();
    }

    [HttpGet]
    public async Task<ActionResult> Account(int accountId)
    {
      var accounts = await Client.GetAccounts();
      var account = accounts.SingleOrDefault(a => a.Id == accountId);
      if (account == null)
      {
        return RedirectToAction("Menu");
      }
      ViewBag.Account = account;
      ViewBag.Projects = await account.GetBasecamps();
      return View();
    }
  }
}