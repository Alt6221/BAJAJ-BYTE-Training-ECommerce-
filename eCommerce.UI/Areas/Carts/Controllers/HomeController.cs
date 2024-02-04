using eCommerce.DAL;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Options;
namespace eCommerce.UI.Areas.Carts.Controllers
{
    [Area("Carts")]
    public class HomeController : Controller
    {
        //URL Carts/Home/MyCart
        private readonly INewCartRepository _newCartRepository;
        private readonly ICommonRepository<CartDetail> _cartDetailRepository;
		private readonly StripeSettings _stripeSettings;
		public string sessionId { get; set; }

		public IActionResult Index()
		{
			return View();
		}

		public HomeController(INewCartRepository newCartRepository, ICommonRepository<CartDetail> cartDetailRepository,IOptions<StripeSettings> stripeSettings)
        {
            _newCartRepository = newCartRepository;
            _cartDetailRepository = cartDetailRepository;
			_stripeSettings = stripeSettings.Value;
		}

        public async Task<IActionResult> MyCart()
        {
            var myCartItems = await _newCartRepository.GetCartDetails((int)HttpContext.Session.GetInt32("CartId"));
            return View(myCartItems);
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            //Please check do you have customerid in session. If not, redirect the user to the login page.
            //After successful login, store the customer Id into session variable and comeback to AddToCart action method.
            HttpContext.Session.SetInt32("CustomerId", 1);
            if(HttpContext.Session.GetInt32("CartId")==null)
            {
                int cartId = await _newCartRepository.GenerateNewCart((int)HttpContext.Session.GetInt32("CustomerId"));
                HttpContext.Session.SetInt32("CartId",cartId);
            }
            
            await _cartDetailRepository.InsertAsync(new()
            {
                CartId = (int)HttpContext.Session.GetInt32("CartId"),
                ProductId = id
            });
            return RedirectToAction("MyCart");
        }
		public async Task<IActionResult> RemoveFromCart(int Id)
		{
            await _cartDetailRepository.DeleteAsync(Id) ;
			return RedirectToAction("MyCart");
		}
		public async Task<IActionResult> CreateCheckoutSessionAsync(string amount)
		{
			var myCartItems = await _newCartRepository.GetCartDetails((int)HttpContext.Session.GetInt32("CartId"));

			var currency = "inr"; // Currency code

			StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
			var options = new SessionCreateOptions
			{
				LineItems = myCartItems.Select(item => new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.TotalAmount * 100),
						Currency = currency, // Change this to your currency
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.ProductName,
						},
					},
					Quantity = 1,
				}).ToList(),
				Mode = "payment",
				SuccessUrl = Url.Action("OrderSuccess", "Home", new { area = "Carts" }, HttpContext.Request.Scheme),
				CancelUrl = Url.Action("OrderCancelled", "Home", new { area = "Carts" }, HttpContext.Request.Scheme),
			};
			var service = new SessionService();
			var session = service.Create(options);
			sessionId = session.Id;
			return Redirect(session.Url);
		}

		public async Task<IActionResult> success()
		{
			// Handle successful payment, e.g., update order status
			return View("Index");
		}

		public IActionResult cancel()
		{
			// Handle cancelled payment, e.g., show a message to the user
			return View("Index");
		}


	}
}
