using FurStore.Common.Enums;
using FurStore.Data;
using FurStore.Data.ServicesBase;
using FurStore.Models.Store;
using Microsoft.EntityFrameworkCore;

namespace FurStore.Services.FurnitureServices
{
    public class FurnitureService : EntityRepository<Furniture>, IFurnitureService
    {
        private readonly AppDbContext _context;

        public FurnitureService(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Furniture>> GetBySearchStringAndFurnitureTypeAsync(string? searchString, string? furnitureType)
        {
            IQueryable<Furniture> queryForFurnitures = _context.Furnitures;
            if (searchString != null)
            {
                queryForFurnitures = queryForFurnitures.Where(fur => fur.Title.Contains(searchString) || fur.Description.Contains(searchString));
            }
            if (furnitureType != null && furnitureType != "All")
            {
                FurnitureTypes type = (FurnitureTypes)Enum.Parse(typeof(FurnitureTypes), furnitureType);
                queryForFurnitures = queryForFurnitures.Where(fur => fur.FurnitureType == type);
            }

            return await queryForFurnitures.ToListAsync();
        }
    }
}
