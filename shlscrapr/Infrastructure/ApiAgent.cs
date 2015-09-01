using System.Collections.Generic;
using Newtonsoft.Json;

namespace shlscrapr.Infrastructure
{
    public class ApiAgent<T>
    {
        private readonly string _url;
        private readonly HttpResourceReader _reader;

        public ApiAgent(string url)
        {
            _url = url;
            _reader = new HttpResourceReader();
        }

        public T GetModel(int indexItem)
        {
            var itemUrl = string.Format(_url, indexItem);
            var modelJson = _reader.Read(itemUrl);
            return JsonConvert.DeserializeObject<T>(modelJson);
        }
    }
}