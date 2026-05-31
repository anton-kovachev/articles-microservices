using Blocks.Core;
using Blocks.Domain;
using Review.Domain.Invitations.Enums;

namespace Review.Domain.Invitations;

public partial class ReviewInvitation
{
    public void Accept()
    {
        if (Status != InvitationStatus.Open)
            throw new DomainException("Invitation is not open to anyomore.");

        if (IsExpired)
            throw new DomainException("Invitation is expired.");

        //TODO: Consider adding an InvitationAccepted domain event
        this.Status = InvitationStatus.Accepted;
    }

    public void Decline()
    {
        if (Status != InvitationStatus.Open)
            throw new DomainException("Invitation is not open to anyomore.");

        //TODO: Consider adding an InvitationDeclined domain event
        this.Status = InvitationStatus.Decline;
    }
}
