using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace VkOAuth.Models
{
    public class User
    {
        public User() { }

        public User(IPAddress ip, long vkId, string token)
        {
            Ip = ip;
            VkId = vkId;
            Token = token;
        }

        [Key]
        [Required, MinLength(4), MaxLength(16)]
        public byte[] IPAddressBytes { get; set; }

        [NotMapped]
        public IPAddress Ip
        {
            get => new IPAddress(IPAddressBytes);
            set => IPAddressBytes = value.GetAddressBytes();
        }

        [Required]
        public string Token { get; set; }

        [Required]
        public long VkId { get; set; }

        [NotMapped]
        public bool IsValid => Ip != null && !string.IsNullOrWhiteSpace(Token) && VkId != 0;
    }
}