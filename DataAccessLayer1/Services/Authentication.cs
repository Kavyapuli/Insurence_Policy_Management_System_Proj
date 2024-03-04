using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public static class Authentication
    {
        private static readonly InsuranceDbContext context = new InsuranceDbContext();


        public static bool VerifyAdminCredentials(string username, string password)
        {
            // check for username and password in the db
            var admin = context.Admins.FirstOrDefault(a => a.UserName == username);

            if (admin != null)
            {
                if (admin.Password == password)
                {
                    return true;
                }
            }

            return false; // Invalid username or password
        }
        public static bool VerifyCustomerCredentials(string username, string password)
        {

            var cutomer = context.Customers.FirstOrDefault(a => a.UserName == username);

            if (cutomer != null)
            {
                if (cutomer.Password == password)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
