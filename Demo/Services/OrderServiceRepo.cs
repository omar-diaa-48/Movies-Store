using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Services
{
    public class OrderServiceRepo : IOrderRepository
    {
        private readonly MovieStoreDBContext context;

        public OrderServiceRepo(MovieStoreDBContext _context)
        {
            context = _context;
        }
    }
}
