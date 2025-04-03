using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApp.Common.Constants
{
    public static class UserRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Agent = "Agent";

        public static readonly string[] AllRoles = { SuperAdmin, Admin, Agent };
    }
}