using System;

namespace Tsarev.Basecamp3.ApiProxy
{
  public enum BasecampAccountType
  {
    Basecamp3
  }

  public class BasecampAccount
  {
    public BasecampAccountType AccountType { get; }
    public string Name { get; }
    public Uri SiteHref { get; }
    public int Id { get; }
    internal Uri ApiHref { get; }
    internal IBasecampClient Client { get; }
    internal BasecampAccount(BasecampAccountResult acc, IBasecampClient client)
    {
      AccountType = BasecampAccountType.Basecamp3;
      Name = acc.name;
      ApiHref = new Uri(acc.href);
      SiteHref = new Uri(acc.app_href);
      Id = acc.id;
      Client = client;
    }
  }


  internal class BasecampAccountResult
  {
    public string product { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public string href { get; set; }
    public string app_href { get; set; }
  }
}