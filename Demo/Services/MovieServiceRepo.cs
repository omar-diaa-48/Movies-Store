using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Services
{
    public class MovieServiceRepo : IMovieRepository
    {
        private readonly MovieStoreDBContext context;

        public MovieServiceRepo(MovieStoreDBContext _context)
        {
            context = _context;
        }
    }
}
