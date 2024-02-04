using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.DAL
{
    public interface ICommonRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetDetailAsync(int id);
        Task<int> InsertAsync(T item);
        Task<int> UpdateAsync(T item);
        Task<int> DeleteAsync(int id);
    }
}
