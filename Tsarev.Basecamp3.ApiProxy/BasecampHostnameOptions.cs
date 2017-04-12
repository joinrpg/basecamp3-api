using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsarev.Basecamp3.ApiProxy
{
  /// <summary>
  /// This class could be used to mock basecamp in your application
  /// </summary>
  public class BasecampHostnameOptions
  {
    [NotNull]
    public string LaunchpadHostname { get; }
    public BasecampHostnameOptions([NotNull] string launchpad = "https://launchpad.37signals.com")
    {
      LaunchpadHostname = launchpad;
    }
  }
}
