using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }

       [HttpGet]
       public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
       {
            var products = await _context.Products.ToListAsync();
            if (products == null)
                return NotFound();

            return Ok(products);
       }

       [HttpGet("{id}")]
       public async Task<ActionResult<Product>> GetProduct(int id)
       {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound($"The product with id {id} Not Exist");

            return Ok(product);
       }

    }
}
