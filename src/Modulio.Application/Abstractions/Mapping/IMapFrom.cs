using AutoMapper;

namespace Modulio.Application.Abstractions.Mapping
{
    /// <summary>
    /// Marker interface for types that can be mapped from <typeparamref name="T"/>.
    /// Used for AutoMapper registration.
    /// </summary>
    /// <typeparam name="T">Source type</typeparam>
    public interface IMapFrom<T>
    {
        void MapFrom(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
