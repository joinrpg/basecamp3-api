using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsarev.Basecamp3.ApiProxy
{
    public class BasecampCredentials
    {
      public string ClientId { get; set; }
      public string ClientSecret { get; set; }
      public Uri RedirectUri { get; set; }
    }
}
