using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Services
{
    public interface IApplicationUserRepository
    {
        public List<ApplicationUser> AllCustomers();
        public ApplicationUser CustomerDetails(int id);
        public void AddCustomer(ApplicationUser customer);
        public void UpdateCustomer(int id);
        public void DeleteCustomer(int id);
    }
}
