using System.Net;
using System.Text;

namespace shlscrapr.Infrastructure
{
    public class HttpResourceReader
    {
        public string Read(string uri)
        {
            using (var webClient = new WebClient() {Encoding = Encoding.UTF8})
            {
                return webClient.DownloadString(uri);
            }
        }
    }
}