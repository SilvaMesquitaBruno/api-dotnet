using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers {

    [Route("products")]
    public class ProductController : ControllerBase {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices]DataContext context) {
            var products = await context.Products.Include(p => p.Category).AsNoTracking().ToListAsync();

            return Ok(products);
        }
    
        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById([FromServices]DataContext context, int id) {
            var product = await context.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync();

            return Ok(product);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices]DataContext context, int id) {
            var product = await context
                .Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.CategoryId == id)
                .ToListAsync();

            return Ok(product);
        }
    
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Product>>> Post([FromServices]DataContext context, [FromBody]Product model) {
        
            if(ModelState.IsValid) {
                context.Products.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);
            } else {
                return BadRequest(ModelState);
            }

        }
    }
}
