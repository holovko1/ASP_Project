using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAlina.Data;
using WebAlina.Models.Category;
using WebAlina.Models.Product;

namespace WebAlina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        public readonly AlinaDbContext _context;
        public readonly IConfiguration _configuration;

        public ProductsController (AlinaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await _context.Products
                .Select(x => new ProductItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    //Image = x.ProductImages,
                })
                .ToListAsync();
            return Ok(list);
        }
    }
}
