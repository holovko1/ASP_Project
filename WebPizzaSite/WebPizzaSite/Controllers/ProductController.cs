﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Models.Product;

namespace WebPizzaSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly PizzaDbContext _pizzaDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public ProductController(PizzaDbContext pizzaDbContext, IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _pizzaDbContext = pizzaDbContext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var list = _pizzaDbContext.Products
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var catList = _pizzaDbContext.Categories
                .Select(x => new { Value = x.Id, Text = x.Name })
                .ToList();

            ProductCreateViewModel model = new ProductCreateViewModel();
            model.CategoryList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(catList, "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prod = new ProductEntity
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId,
            };

            await _pizzaDbContext.Products.AddAsync(prod);
            await _pizzaDbContext.SaveChangesAsync();
            if (model.Photos != null)
            {
                int i = 0;

                foreach (var img in model.Photos)
                {
                    string ext = System.IO.Path.GetExtension(img.FileName);
                    string fileName = Guid.NewGuid().ToString() + ext;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    var imgEntity = new ProductImageEntity
                    {
                        Name = fileName,
                        Priority = i++,
                        Product = prod,
                    };
                    _pizzaDbContext.ProductImages.Add(imgEntity);
                    _pizzaDbContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var entity = _pizzaDbContext.Products.Where(p => p.Id == id).Include(p => p.ProductImages).FirstOrDefault();
                if (entity.ProductImages != null)
                {
                    foreach (var img in entity.ProductImages)
                    {
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", img.Name);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
            _pizzaDbContext.Products.Remove(entity);
            _pizzaDbContext.SaveChanges();
            return Ok();
        }
    }
}
