using Newtonsoft.Json;
using System.Dynamic;
using System.IO;
using System.Net;

namespace VkOAuth.Models
{
    public static class WebRequestExtensions
    {
        public static dynamic GetBodyObject(string requestString)
        {
            var request = WebRequest.Create(requestString);
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var body = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<ExpandoObject>(body);
            }
        }
    }
}