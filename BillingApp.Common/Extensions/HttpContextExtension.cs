//// BillingApp.Common/Extensions/HttpContextExtensions.cs
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Net.Http;

//namespace BillingApp.Common.Extensions
//{
//    public static class HttpContextExtensions
//    {
//        public static Guid GetUserId(this HttpContext context)
//        {
//            var userId = context.Session.GetString("UserId");
//            if (Guid.TryParse(userId, out var result))
//            {
//                return result;
//            }
//            throw new UnauthorizedAccessException("Invalid user ID in session");
//        }

//        public static string GetUserRole(this HttpContext context)
//        {
//            return context.Session.GetString("UserRole") ??
//                   throw new UnauthorizedAccessException("User role not found");
//        }
//    }
//}