using Microsoft.EntityFrameworkCore;

namespace FurStore.Data.ServicesBase
{
    public class EntityRepository<T> : IEntityRepository<T>
        where T : class, IEntity, new()
    {
        private readonly AppDbContext _context;

        public EntityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T item)
        {
            var entry = _context.Entry<T>(item);
            entry.State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FirstAsync(item => item.Id == id);
        }

        public async Task UpdateAsync(T item)
        {
            var entry = _context.Entry<T>(item);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
