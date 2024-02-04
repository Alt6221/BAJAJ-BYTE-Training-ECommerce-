using eCommerce.Models;

namespace eCommerce.DAL
{
    public interface INewCartRepository
    {
        public Task<int> GenerateNewCart(int customerId);
        public Task RemoveCartDetails(int cartId);
        public Task<List<MyCartVM>>GetCartDetails(int cartId);
    }
}
