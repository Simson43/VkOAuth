using System.Data.Entity;
using VkOAuth.Models;

namespace VkAuth
{
    public class VkAuthContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public VkAuthContext(): 
            base("Connect")
        { }
    }
}
