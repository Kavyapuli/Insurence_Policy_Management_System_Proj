using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
     public interface CustomerInterface
    {
        // Create Customer
        Customer CreateCustomer(Customer customer);
        int customerSaveChanges();

        // Read Customer data
        Customer GetCustomerByUserName(string userName);
        Customer GetCustomerByUserNamePhone(string UserName, string PhoneNumber);

        bool CustomerExistsEmail(string Email);

        Customer GetCustomerById(int customerId);
        IEnumerable<Customer> GetAllCustomers();
        bool CustomerExists(string userName);
        // Update Customer data
        Customer UpdateCustomer(Customer customer);

        // Delete Customer data
        Customer DeleteCustomer(int customerId);
    }
}
