using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.DTOs.AutoMapper.IMapFrom", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Mapping;

public interface IMapFrom<T>
{
    void MappingFrom(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
