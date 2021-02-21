using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Services
{
    public class CustomerServiceRepo : ICustomerRepository
    {
        private readonly MovieStoreDBContext context;

        public CustomerServiceRepo(MovieStoreDBContext _context)
        {
            context = _context;
        }
        public void AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public List<Customer> AllCustomers()
        {
            throw new NotImplementedException();
        }

        public Customer CustomerDetails(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(int id)
        {
            throw new NotImplementedException();
        }
    }
}
