using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using VkOAuth.Models;

namespace VkOAuth.Controllers
{
    public class AuthController : Controller
    {
        private static readonly UserRepository userRepo = new UserRepository();
        private static readonly VkApi vk =
            new VkApi(7079112, "ob3eV55pzICUycyCP55z", $"http://simsonnu.somee.com/Auth/SetCode&", "5.101");
        
        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                var ip = GetUserIP();
                if (userRepo.Contains(ip))
                    return GetHelloView(userRepo.Find(ip));
                ViewBag.RequestString = vk.GetAuthUri();
                return View();
            }
            catch { return View("Error"); }
        }

        [HttpGet]
        public ActionResult SetCode(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return View("Login");
                var user = GetUser(code);
                if (!user.IsValid)
                    return View("Login");
                userRepo.Add(user);
                return GetHelloView(user);
            }
            catch { return View("Error"); }
        }

        private ActionResult GetHelloView(User user)
        {
            try
            {
                ViewBag.Name = vk.GetName(user);
                ViewBag.friends = vk.GetFriends(user);
                return View("Hello");
            }
            catch (TokenExpiredException)
            {
                userRepo.Remove(user.Ip);
                return View("Login");
            }
        }

        private User GetUser(string code)
        {
            string token = vk.GetAccessToken(code, out long vkId);
            return new User(GetUserIP(), vkId, token);
        }
        
        private IPAddress GetUserIP()
        {
            string ip;
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            ip = !string.IsNullOrEmpty(ipList)
                ? ipList.Split(',')[0]
                : Request.ServerVariables["REMOTE_ADDR"];
            return IPAddress.Parse(ip);
        }
    }
}