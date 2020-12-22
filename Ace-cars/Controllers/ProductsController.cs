using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ace_cars.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Ace_cars.Helper;
using System.Text;
using Ace_cars.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace Ace_cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;
        public readonly IHubContext<ChangePriceHub> _changePriceHub;

        public ProductsController(ProductsContext context, IHubContext<ChangePriceHub> changePriceHub)
        {
            _context = context;
            _changePriceHub = changePriceHub;
        }
        [Authorize]
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var Products = await _context.Products.ToListAsync();
            return Products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var Product = await _context.Products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Product;
        }
        [Authorize]
        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [Authorize]
        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {


            _context.Products.Add(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (ProductExists(product.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw e;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        [Authorize]
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            
            return _context.Products.Any(e => e.Id == id);
        }
        private static double GetRandomNumber(double min, double max)
        {
            var randomInstance = new RandomNumbers();
            double random = randomInstance.NextDouble(min, max);
            return random;
        }
        private async Task<Product> updateRondomProduct()
        {
            var product = _context.Products.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            var randomPrice = GetRandomNumber(500, 10000);
            product.Price = Math.Round(randomPrice,2) ;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }
        [HttpGet("process")]
        //[Route("process")]
        public void  process()
            {
            var product = updateRondomProduct();
            var productJson = JsonSerializer.Serialize(product);
            _changePriceHub.Clients.All.SendAsync("changeProductPrice", productJson);
        }




    }
}
