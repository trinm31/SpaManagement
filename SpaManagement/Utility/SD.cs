using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.Utility
{
    public static class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Staff = "Staff";
        public const string ssCustomerID = "CustomerID";
        public const string ssProductID = "ProductID";
    }
}