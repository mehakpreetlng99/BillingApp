﻿
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BillingApp.Web.Middleware
{
    public class SessionRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            if (context.User.Identity.IsAuthenticated && string.IsNullOrEmpty(context.Session.GetString("UserRole")))
            {
                
                var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(role))
                {
                    
                    context.Session.SetString("UserRole", role);

                    Console.WriteLine($"🔹 Role '{role}' stored in session.");
                }
                else
                {
                    
                    Console.WriteLine("⚠️ No role found in claims.");
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    context.Session.SetString("UserId", userId);
                    Console.WriteLine($"🔹 User ID '{userId}' stored in session.");
                }
                else
                {
                    Console.WriteLine("⚠️ No User ID found in claims.");
                }
            }
            else
            {
                
                Console.WriteLine("⚠️ User is not authenticated or session already contains a role.");
            }

            await _next(context);
        }
    }
}

