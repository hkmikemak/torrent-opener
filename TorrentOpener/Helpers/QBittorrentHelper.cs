using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace TorrentOpener.Helpers {

  internal static class QBittorrentHelper {

    #region Public Methods

    public static async Task AddTorrent(string source, string category) {
      if (source.ToLower().StartsWith("magnet") || source.ToLower().StartsWith("http")) {
        await AddUrl(source, category);
      } else {
        await AddFile(source, category);
      }
    }

    #endregion Public Methods

    #region Private Methods

    private static async Task AddFile(string fullpath, string category) {
      string baseUrl = ConfigurationManager.AppSettings["Url"];

      Cookie cookie = await Login();
      (await baseUrl .AppendPathSegment("/api/v2/torrents/add")
                    .WithCookie(cookie)
                    .PostMultipartAsync(
                      content => content.AddFile("torrents", fullpath)
                                        .AddString("category", category)
                    )).Dispose();
      await Logout(cookie);
    }

    private static async Task AddUrl(string urls, string category) {
      string baseUrl = ConfigurationManager.AppSettings["Url"];
      Cookie cookie = await Login();
      (await baseUrl .AppendPathSegment("/api/v2/torrents/add")
                    .WithCookie(cookie)
                    .PostMultipartAsync(
                      content => content.AddString("urls", urls)
                                        .AddString("category", category)
                    )).Dispose();
    }

    private static async Task<Cookie> Login() {
      string baseUrl = ConfigurationManager.AppSettings["Url"];

      var postdata = new {
        username = ConfigurationManager.AppSettings["Username"],
        password = ConfigurationManager.AppSettings["Password"],
      };

      using (HttpResponseMessage response = await baseUrl.AppendPathSegment("/api/v2/auth/login").PostUrlEncodedAsync(postdata)) {
        string value = response.Headers.Where(i => "set-cookie".Equals(i.Key, StringComparison.InvariantCultureIgnoreCase)).First().Value.First();
        string[] cookiepath = value.Split(';').First().Split('=').Select(i => i.Trim()).ToArray();
        return new Cookie(cookiepath[0], cookiepath[1]);
      }
    }

    private static async Task Logout(Cookie cookie) {
      string baseUrl = ConfigurationManager.AppSettings["Url"];
      (await baseUrl.AppendPathSegment("/api/v2/auth/logout").WithCookie(cookie).PostAsync(null)).Dispose();
    }

    #endregion Private Methods
  }
}
