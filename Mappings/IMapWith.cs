using AutoMapper;

namespace FurStore.Mappings
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile);
    }
}
