using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IStoreRepository repository;

        public ProductController(IStoreRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string? category = null, int productPage = 1)
        {
            int PageSize = 3;

            var productsQuery = repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductId);

            var model = new ProductsListViewModel
            {
                Products = productsQuery
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null
                        ? repository.Products.Count()
                        : repository.Products.Count(p => p.Category == category)
                },
                CurrentCategory = category ?? string.Empty
            };

            ViewBag.Categories = repository.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return View(model);
        }
    }
}