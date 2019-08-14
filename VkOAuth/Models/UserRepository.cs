using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VkAuth;

namespace VkOAuth.Models
{
    public class UserRepository
    {
        private VkAuthContext db = new VkAuthContext();

        public User Find(IPAddress ip)
        {
            if (ip == null)
                throw new NullReferenceException();
            return db.Users.Find(ip.GetAddressBytes());
        }

        public User Add(User user)
        {
            if (user == null)
                throw new NullReferenceException();
            var newUser = db.Users.Add(user);
            db.SaveChanges();
            return newUser;
        }

        public bool Contains(IPAddress ip)
        {
            if (ip == null)
                throw new NullReferenceException();
            return db.Users.Find(ip.GetAddressBytes()) != null;
        }

        public void Remove(IPAddress ip)
        {
            var user = Find(ip);
            if (user == null)
                return;
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public List<User> GetAll()
        {
            return db.Users.ToList();
        }
    }
}