using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.DAL
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        private readonly EcommerceDbContext _context;
        
        public CommonRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetDetailAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> InsertAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            return await _context.SaveChangesAsync(); //gives the number of rows affected
        }

        public async Task<int> UpdateAsync(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return await _context.SaveChangesAsync(); //returns the number of rows affected
        }
        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return 0;
            }
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync(); //returns the number of rows affected
        }
    }
}
