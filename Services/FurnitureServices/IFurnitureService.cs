using FurStore.Data.ServicesBase;
using FurStore.Models.Store;

namespace FurStore.Services.FurnitureServices
{
    public interface IFurnitureService : IEntityRepository<Furniture>
    {
        public Task<IEnumerable<Furniture>> GetBySearchStringAndFurnitureTypeAsync(string? searchString, string? furnitureType);
    }
}
