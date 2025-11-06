using Microsoft.AspNetCore.Mvc;
using Printstream.Infrastructure;

namespace Printstream.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
    }
}