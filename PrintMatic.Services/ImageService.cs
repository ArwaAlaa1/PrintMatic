using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Services
{
    public static class ImageService
    {
        public static  string ConvertUrlsToJson(List<string> urls)
        {
            return JsonConvert.SerializeObject(urls);
        }
        public static List<string> ConvertJsonToUrls(string json)
        {
            return JsonConvert.DeserializeObject<List<string>>(json);
        }
    }
}
