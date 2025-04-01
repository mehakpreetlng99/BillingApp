
using Microsoft.AspNetCore.Identity;

namespace BillingApp.Models
{
   public class User:IdentityUser
    {
        public string FullName { get; set; }=string.Empty;
    }
}
