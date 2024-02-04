using eCommerce.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.DAL
{
    public class NewCartRepository : INewCartRepository
    {
        private readonly EcommerceDbContext _context;

        public NewCartRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> GenerateNewCart(int customerId)
        {
            var customerIdParameter = new SqlParameter()
            {
                ParameterName = "@p_CustomerId",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = customerId
            };
            var cartIdParameter = new SqlParameter()
            {
                ParameterName = "@p_CartId",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            await _context.Database.ExecuteSqlRawAsync("EXEC GenerateNewCart @p_CustomerId, @p_CartId OUTPUT", customerIdParameter, cartIdParameter);
            return Convert.ToInt32(cartIdParameter.Value);
        }
		public async Task RemoveCartDetails(int cartId)
		{
			var cartIdParameter = new SqlParameter()
			{
				ParameterName = "@p_CartId",
				SqlDbType = System.Data.SqlDbType.Int,
				Value = cartId
			};

			await _context.Database.ExecuteSqlRawAsync("EXEC RemoveCartDetails @p_CartId", cartIdParameter);
		}


		public async Task<List<MyCartVM>> GetCartDetails(int cartId)
        {
            var cartDetailQuery = from cart in _context.Carts
                                 join
                                 cartDetail in _context.CartDetails
                                 on cart.CartId equals cartDetail.CartId
                                 join
                                 product in _context.Products
                                 on cartDetail.ProductId equals product.ProductId
                                 where cart.CartId == cartId
                                 select new MyCartVM()
                                 {
                                     CartDetailId = cartDetail.CartDetailId,
                                     Discount = product.Discount,
                                     ProductName = product.ProductName,
                                     Quantity = cartDetail.Quantity,
                                     Size = cartDetail.Size,
                                     UnitPrice = product.UnitPrice,
                                     DiscountedAmount = product.UnitPrice - ((product.UnitPrice * product.Discount) / 100),
                                     CartDate = cart.CartDate,
                                     Picture=product.Picture
                                 };
            return await cartDetailQuery.ToListAsync();
        }
    }
}
