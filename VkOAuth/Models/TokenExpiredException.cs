using System;

namespace VkOAuth.Models
{
    public class TokenExpiredException: Exception
    {
        public override string Message => "Срок действия токена истек. Необходимо получить новый токен.";
    }
}