using Articles.Abstractions.Enums;
using Blocks.Domain;
using ProtoBuf;
using ProtoBuf.Grpc;
using System.ServiceModel;

namespace Auth.Grpc;

[ProtoContract]
public class PersonInfo
{
    [ProtoMember(1)]
    public int Id { get; set; }
    [ProtoMember(2)]
    public string FirstName { get; set; } = default!;
    [ProtoMember(3)]
    public string LastName { get; set; } = default!;
    [ProtoMember(4)]
    public string Email { get; set; } = default!;
    [ProtoMember(5)]
    public Gender Gender { get; set; }
    [ProtoMember(6)]
    public string? Honorific { get; set; }
    [ProtoMember(7)]
    public string? PictureUrl { get; set; }
    [ProtoMember(8)]
    public ProfessionalProfile? ProfessionalProfile { get; set; }
    [ProtoMember(9)]
    public int? UserId { get; set; }
}

[ProtoContract]
public class ProfessionalProfile
{
    [ProtoMember(1)]
    public string Position { get; set; } = default!;
    [ProtoMember(2)]
    public string CompanyName { get; set; } = default!;
    [ProtoMember(3)]
    public string Affiliation { get; set; } = default!;
}

[ProtoContract]
public class GetPersonByIdRequest : IAuditableAction
{
    [ProtoMember(1)]
    public int UserId { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedById { get; set; }

    public string Action => string.Empty;
}

[ProtoContract]
public class GetPersonResponse
{
    [ProtoMember(1)]
    public PersonInfo PersonInfo { get; set; } = default!;
}

[ServiceContract]
public interface IPersonService
{
    [OperationContract]
    ValueTask<GetPersonResponse> GetPersonByUserIdAsync(GetPersonByIdRequest request, CallContext context = default);
}