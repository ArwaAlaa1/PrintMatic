using Microsoft.EntityFrameworkCore;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository
{
	public class GenericRepository<T>: IGenericRepository<T> where T : BaseEntity
    {
        private readonly PrintMaticContext _context;

        public GenericRepository(PrintMaticContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public  void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
        public void Delete(T entity)
        { 
            _context.Remove(entity);
        }
    }
}
