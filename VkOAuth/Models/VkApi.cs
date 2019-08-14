using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace VkOAuth.Models
{
    public class VkApi
    {
        private readonly long clientId;
        private readonly string clientSecret;
        private readonly string redirectUri;
        private readonly string version;

        public VkApi(long clientId, string clientSecret, string redirectUri, string version)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUri = redirectUri;
            this.version = version;
        }

        public string GetName(User user)
        {
            string requestString = "https://api.vk.com/method/users.get?" +
                               $"user_id={user.VkId}&" +
                               "fields=name&" +
                               $"access_token={user.Token}&" +
                               $"v={version}";

            var obj = WebRequestExtensions.GetBodyObject(requestString);
            return HandleFucn(obj, new Func<dynamic>(() => 
                $"{obj.response[0].first_name} {obj.response[0].last_name}"));
        }

        public List<string> GetFriends(User user)
        {
            string requestString = "https://api.vk.com/method/friends.get?" +
                               $"user_id={user.VkId}&" +
                               "order=random&" +
                               "fields=name&" +
                               "count=5&" +
                               $"access_token={user.Token}&" +
                               $"v={version}";

            var obj = WebRequestExtensions.GetBodyObject(requestString);

            return HandleFucn(obj, new Func<dynamic>(() =>
                new List<dynamic>(obj.response.items).Select(x => $"{x.first_name} {x.last_name}").ToList()));
        }

        private dynamic HandleFucn(dynamic obj, Func<dynamic> func)
        {
            try
            {
                return func();
            }
            catch
            {
                var code = obj.error.error_code;
                if (code == 5 || code == 18 || code == 28)
                    throw new TokenExpiredException();
                throw;
            }
        }

        public string GetAuthUri()
        {
            return "https://oauth.vk.com/authorize?" +
                $"client_id={clientId}&" +
                "scope=friends,offline&" +
                "display=page&" +
                "response_type=code&" +
                $"redirect_uri={redirectUri}&" +
                $"v={version}";
        }

        public string GetAccessToken(string code, out long vkId)
        {
            string requestString = "https://oauth.vk.com/access_token?" +
                $"client_id={clientId}&" +
                $"client_secret={clientSecret}&" +
                $"redirect_uri={redirectUri}&" +
                $"code={code}";
            var obj = WebRequestExtensions.GetBodyObject(requestString);
            vkId = obj.user_id;
            return obj.access_token;
        }
    }
}