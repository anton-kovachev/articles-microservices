using Auth.Grpc;
using Mapster;

namespace Auth.Domain.Persons;

public class GrpcTypeAdapterConfig : TypeAdapterConfig
{
    public GrpcTypeAdapterConfig()
    {
        this.NewConfig<Person, PersonInfo>()
            .Map(dest => dest.Honorific, source => source.Honorific == null ? null : source.Honorific.Value)
            .IgnoreNullValues(true);

        this.NewConfig<ValueObjects.ProfessionalProfile, ProfessionalProfile>();
    }
}
