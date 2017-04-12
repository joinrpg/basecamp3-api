using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsarev.Basecamp3.ApiProxy
{
  public static class Basecamps
  {
    public static async Task<IReadOnlyCollection<BasecampProject>> GetBasecamps(this BasecampAccount account)
    {
      var result = await account.Client.Get<List<BasecampProjectResult>>(account.ApiHref.AbsoluteUri + "/projects.json");
      var projects = new List<BasecampProject>();
      foreach (var item in result)
      {
        projects.Add(new BasecampProject(item));
      }
      return projects;
    }
  }

  public enum BasecampStatus
  {
    Active, Archived, Trashed
  }

  public static class BasecampStatusExtensions
  {
    public static BasecampStatus ToBasecampStatus(this string status)
    {
      switch (status)
      {
        case "active": return BasecampStatus.Active;
        case "archived": return BasecampStatus.Archived;
        case "trashed": return BasecampStatus.Trashed;
        default: throw new ArgumentOutOfRangeException(nameof(status));
      }
    }
  }

  internal class DockItemResult
  {
    public object id { get; set; }
    public string title { get; set; }
    public string name { get; set; }
    public bool enabled { get; set; }
    public int? position { get; set; }
    public string url { get; set; }
    public string app_url { get; set; }
  }

  internal class BasecampProjectResult
  {
    public int id { get; set; }
    public string status { get; set; }
    public DateTimeOffset created_at { get; set; }
    public DateTimeOffset updated_at { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string bookmark_url { get; set; }
    public string url { get; set; }
    public string app_url { get; set; }
    public List<DockItemResult> dock { get; set; }
    public bool bookmarked { get; set; }
  }
  public class BasecampProject
  {
    public int Id { get; }
    public BasecampStatus Status { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    public string Name { get; }
    public string Description { get; }
    public Uri BookmarkUri { get; }
    internal Uri ApiUri { get; }
    public Uri VisibleUri { get; }
    //TODO: Docked items

    internal BasecampProject(BasecampProjectResult data)
    {
      Id = data.id;
      Status = data.status.ToBasecampStatus();
      CreatedAt = data.created_at;
      UpdatedAt = data.updated_at;
      Name = data.name;
      Description = data.description;
      BookmarkUri = new Uri(data.bookmark_url);
      ApiUri = new Uri(data.url);
      VisibleUri = new Uri(data.app_url);
    }
  }
}
