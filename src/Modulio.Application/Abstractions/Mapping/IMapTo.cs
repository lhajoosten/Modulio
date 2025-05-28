using AutoMapper;

namespace Modulio.Application.Abstractions.Mapping
{
    /// <summary>
    /// Marker interface for types that can be mapped to <typeparamref name="T"/>.
    /// Used for AutoMapper registration.
    /// </summary>
    /// <typeparam name="T">Destination type</typeparam>
    public interface IMapTo<T>
    {
        void MapTo(Profile profile) => profile.CreateMap(GetType(), typeof(T));
    }
}
