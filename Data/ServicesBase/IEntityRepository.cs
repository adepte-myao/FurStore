namespace FurStore.Data.ServicesBase
{
    public interface IEntityRepository<T> where T : IEntity
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetByIdAsync(Guid id);

        public Task CreateAsync(T item);

        public Task UpdateAsync(T item);

        public Task DeleteAsync(T item);
    }
}
