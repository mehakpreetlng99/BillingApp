namespace BillingApp.Web.Middleware
{
    // HttpContextExtensions.cs
    public static class HttpContextExtensions
    {
        public static string GetUserRole(this HttpContext context)
        {
            return context.Session.GetString("UserRole");
        }

        public static Guid GetUserId(this HttpContext context)
        {
            var userId = context.Session.GetString("UserId");
            if (Guid.TryParse(userId, out var result))
            {
                return result;
            }
            throw new UnauthorizedAccessException("Invalid user ID in session");
        }
    }
}
