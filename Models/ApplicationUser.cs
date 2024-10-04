using System;
using Microsoft.AspNetCore.Identity;

namespace API_TEMPERATURA_MAXIMA.Models
{
   public class ApplicationUser:IdentityUser
    {
        public String? Cpf { get; set; }
    }
}
