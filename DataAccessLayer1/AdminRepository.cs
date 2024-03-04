using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class AdminRepository:AdminInterface
    {
        private readonly InsuranceDbContext db;

        public AdminRepository(InsuranceDbContext context)
        {
            db = context;
        }

        // Create Admin
        public Admin CreateAdmin(Admin admin)
        {
            db.Admins.Add(admin);
            db.SaveChanges();
            return admin;
        }

        // Read Admin data
        public Admin GetAdminById(int adminId)
        {
            return db.Admins.Find(adminId);
        }
        public bool AdminExists(string userName)
        {
            return db.Admins.Any(a => a.UserName == userName);
        }

        public bool AdminExistsEmail(string UserEmail)
        {
            return db.Admins.Any(a => a.UserName == UserEmail);
        }
        public IEnumerable<Admin> GetAllAdmins()
        {
            return db.Admins.ToList();
        }

        // Update Admin data
        public Admin UpdateAdmin(Admin admin)
        {
            var existingAdmin = db.Admins.Find(admin.Id);

            if (existingAdmin != null)
            {
                // Update the properties of the existing admin with the values from the input admin
                existingAdmin.FirstName = admin.FirstName;
                existingAdmin.LastName = admin.LastName;
                existingAdmin.EmailAddress = admin.EmailAddress;
                existingAdmin.PhoneNumber = admin.PhoneNumber;
                existingAdmin.UserName = admin.UserName;
                existingAdmin.Password = admin.Password;
                existingAdmin.RoleId = admin.RoleId;

                db.SaveChanges();
            }

            return existingAdmin;
        }

        // Delete Admin data
        public Admin DeleteAdmin(int adminId)
        {
            var admin = db.Admins.Find(adminId);

            if (admin != null)
            {
                db.Admins.Remove(admin);
                db.SaveChanges();
            }

            return admin;
        }

        public  Admin GetAdminByUserName(string userName)
        {
            return db.Admins.FirstOrDefault(x => x.UserName == userName);
        }
        public Admin GetAdminByUserNamePhone(string userName, string phoneNumber)
        {
            return db.Admins.FirstOrDefault(a => a.UserName == userName && a.PhoneNumber == phoneNumber);
        }
        public void SaveAdminchanges()
        {
            db.SaveChanges();
        }
    }
}
