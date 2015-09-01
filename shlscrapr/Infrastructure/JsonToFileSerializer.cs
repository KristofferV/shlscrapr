using System.IO;
using Newtonsoft.Json;
using shlscrapr.Models;

namespace shlscrapr.Infrastructure
{
    public static class JsonToFileSerializer<T> where T : BaseModel
    {
        public static void WriteToFile(T jsonObject, string fileName)
        {
            CreateDirectoryIfMissing(fileName);

            using (var fs = File.Create(fileName))
            {
                using (var sw = new StreamWriter(fs))
                {
                    var settings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new BaseMembersFirstContractResolver()
                    };
                    var json = JsonConvert.SerializeObject(jsonObject, typeof(T), Formatting.Indented, settings);

                    sw.Write(json);
                    sw.Flush();
                }
            }
        }

        private static void CreateDirectoryIfMissing(string fileName)
        {
            var directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}
