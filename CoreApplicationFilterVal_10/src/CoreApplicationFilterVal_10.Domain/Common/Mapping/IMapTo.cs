using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.DTOs.AutoMapper.IMapTo", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Mapping;

public interface IMapTo<T>
{
    void MappingTo(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
}
