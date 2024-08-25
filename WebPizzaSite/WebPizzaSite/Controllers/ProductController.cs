using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using WebPizzaSite.Data;
using WebPizzaSite.Models.Product;

namespace WebPizzaSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly PizzaDbContext _context;
        private readonly IMapper _mapper;
        public ProductController(PizzaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var list = _context.Products
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .ToList();
            return View(list);
        }
    }
}
