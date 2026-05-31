using Blocks.Core.Security;
using Blocks.Domain.ValueObjects;

namespace Review.Domain.Invitations.ValueObjects;

public class InvitationToken : StringValueObject
{
    private InvitationToken(string value) => Value = value;

    public static InvitationToken CreateNew()
        => new InvitationToken(Base64UrlTokenGenerator.Generate());
}
