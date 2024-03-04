using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class CustomerRepository : CustomerInterface
    {
        private readonly InsuranceDbContext db;

        public CustomerRepository(InsuranceDbContext context)
        {
            db = context;
        }

        public Customer GetCustomerByUserName(string userName)
        {
            return db.Customers.FirstOrDefault(x => x.UserName == userName);
        }
        // Create Customer Data
        public Customer CreateCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
            return customer;
        }

        // Read Customer Data
        public Customer GetCustomerById(int customerId)
        {
            return db.Customers.Find(customerId);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return db.Customers.ToList();
        }

        // Update Customer Data
        public Customer UpdateCustomer(Customer customer)
        {
            var existingCustomer = db.Customers.Find(customer.Id);

            if (existingCustomer != null)
            {
                // Update the properties of the existing customer with the values from the input customer
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Email = customer.Email;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.UserName = customer.UserName;
                existingCustomer.Password = customer.Password;
                existingCustomer.RoleId = customer.RoleId;

                db.SaveChanges();
            }

            return existingCustomer;
        }

        // Delete Customer Data
        public Customer DeleteCustomer(int customerId)
        {
            var customer = db.Customers.Find(customerId);

            if (customer != null)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }

            return customer;
        }

        public int customerSaveChanges()
        {
            return db.SaveChanges();
        }
        public bool CustomerExists(string userName)
        {
            return db.Customers.Any(a => a.UserName == userName);
        }

        public Customer GetCustomerByUserNamePhone(string UserName, string PhoneNumber)
        {
            return db.Customers.FirstOrDefault(a => a.UserName == UserName && a.PhoneNumber == PhoneNumber);
        }


        public bool CustomerExistsEmail(string Email)
        {
            return db.Customers.Any(a => a.Email == Email);
        }
    }
}
