using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface AdminInterface
    {
        // Create Admin
        Admin CreateAdmin(Admin admin);

        // Read Admin data
        bool AdminExistsEmail(string UserEmail);
        bool AdminExists(string userName);
        Admin GetAdminById(int adminId);
        Admin GetAdminByUserName(string userName);
        Admin GetAdminByUserNamePhone(string userName, string phoneNumber);
        IEnumerable<Admin> GetAllAdmins();

        // Update Admin data
        Admin UpdateAdmin(Admin admin);

        // Delete Admin data
        Admin DeleteAdmin(int adminId);

        //Save Admin data
        void SaveAdminchanges();

    }
}
