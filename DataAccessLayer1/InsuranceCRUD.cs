using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class InsuranceCRUD
    {
        private readonly InsuranceDbContext db;

        public InsuranceCRUD(InsuranceDbContext context)
        {
            db = context;
        }

        // Admin get,create,update,delete
        public List<Admin> GetAdmins()
        {
            return db.Admins.ToList();
        }

        public Admin GetAdminById(int adminId)
        {
            return db.Admins.Find(adminId);
        }

        public void AddAdmin(Admin admin)
        {
            db.Admins.Add(admin);
            db.SaveChanges();
        }

        public void UpdateAdmin(Admin admin)
        {
              db.Entry(admin).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteAdmin(int adminId)
        {
            Admin admin = db.Admins.Find(adminId);
            if (admin != null)
            {
                db.Admins.Remove(admin);
                db.SaveChanges();
            }
        }

        // Customer get,create,update,delete
        public List<Customer> GetCustomers()
        {
            return db.Customers.ToList();
        }

        public Customer GetCustomerById(int customerId)
        {
            return db.Customers.Find(customerId);
        }

        public void AddCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteCustomer(int customerId)
        {
            Customer customer = db.Customers.Find(customerId);
            if (customer != null)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }
        }

        // Policy operations get,create,update,delete
        public List<Policy> GetPolicies()
        {
            return db.Policies.ToList();
        }

        public Policy GetPolicyById(int policyId)
        {
            return db.Policies.Find(policyId);
        }

        public void AddPolicy(Policy policy)
        {
            db.Policies.Add(policy);
            db.SaveChanges();
        }

        public void UpdatePolicy(Policy policy)
        {
            db.Entry(policy).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeletePolicy(int policyId)
        {
            Policy policy = db.Policies.Find(policyId);
            if (policy != null)
            {
                db.Policies.Remove(policy);
                db .SaveChanges();
            }
        }
    }
}
