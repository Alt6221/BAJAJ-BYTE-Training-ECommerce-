using eCommerce.DAL;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace eCommerce.UI.Areas.Products.Controllers
{
    [Area("Products")]
    public class HomeController : Controller
    {
        private readonly ICommonRepository<Product> _productsrepo;
        private readonly IMemoryCache _productsCache;



        public HomeController(ICommonRepository<Product> productsrepo, IMemoryCache productsCache)
        {
            _productsrepo = productsrepo;
            _productsCache = productsCache;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PageTitle = "eCommerce Products List!";
            ViewBag.PageSubTitle = "Find all the products under single roof";
            if(_productsCache.TryGetValue("eCommerceProducts",out List<Product>products)) //the data will be retrieved from cache
                {
                return View(products);
            }
            var allProducts=await _productsrepo.GetAllAsync(); // if its not present in cache, it connects with the database and fetched the data, shows it in UI.
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(new TimeSpan(0, 10, 0)).SetSlidingExpiration(new TimeSpan(0,1,0)); //data will remain for 1min in cache
            _productsCache.Set("eCommerceProductsCache", allProducts, cacheEntryOptions);
            return View(allProducts);
        }
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.PageTitle = "Details of";
            var product=await _productsrepo.GetDetailAsync(id);
            double discountedAmount = product.UnitPrice- ((product.UnitPrice * product.Discount) / 100);
            ViewBag.Discount=discountedAmount;
            return View(product);
        }
        public async Task<ActionResult> GetParticularCategoryProducts(int id)
        {
            ViewBag.PageTitle = "eCommerce Products List";
            ViewBag.PageSubTitle = "Find all products under single roof using Category ID";
            var products = await _productsrepo.GetAllAsync();
            List<Product> categoryProducts = new List<Product>();
            foreach (var product in products)
            {
                if (product.CategoryId == id)
                {
                    categoryProducts.Add(product);
                }

            }
            return View(categoryProducts);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var result = await _productsrepo.InsertAsync(product);
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
    }
}
