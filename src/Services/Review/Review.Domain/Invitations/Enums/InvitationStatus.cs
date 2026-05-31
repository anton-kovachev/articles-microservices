namespace Review.Domain.Invitations.Enums;

public enum InvitationStatus
{
    Open,
    Accepted, 
    Decline,
    Expired //TODO: Create a daily reocurring job to set the status for the expired inviations
}
