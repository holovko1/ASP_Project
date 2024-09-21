﻿using WebPizzaSite.Models.Helpers;

namespace WebPizzaSite.Models.Product
{
    public class ProductsHomeViewModel
    {
        public List<ProductItemViewModel>? Data { get; set; }

        //Кількість усіх записів в БД (враховуючи параметри пошуку)
        public int Count { get; set; }
        public PaginationViewModel? Pagination { get; set; }
        public ProductSearchViewModel? Search { get; set; }
    }
}
