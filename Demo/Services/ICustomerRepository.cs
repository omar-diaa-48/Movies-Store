using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Services
{
    public interface ICustomerRepository
    {
        public List<Customer> AllCustomers();
        public Customer CustomerDetails(int id);
        public void AddCustomer(Customer customer);
        public void UpdateCustomer(int id);
        public void DeleteCustomer(int id);
    }
}
