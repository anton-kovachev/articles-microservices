using Auth.Grpc;
using MediatR;
using Review.Domain.Reviewers;
using System.Net.WebSockets;

namespace Review.Application.Features.Invitations.AcceptInvitation;

public class AcceptInvitationCommandHandler(
    ReviewDbContext dbContext,
    ArticleRepository _articleRepository, 
    ReviewInvitationRepository _reviewInvitationRepository,
    ReviewerRepository _reviewerRepository,
    IPersonService _personClient) : 
    IRequestHandler<AcceptInvitationCommand, AcceptInvitationResponse>
{
    public async Task<AcceptInvitationResponse> Handle(AcceptInvitationCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdOrThrowAsync(command.ArticleId, cancellationToken);
        var invitation = await _reviewInvitationRepository.GetByTokenOrThrowAsync(command.Token, cancellationToken);

        Reviewer? reviewer = default!;
        if (invitation.UserId != null)
        {
            reviewer = await _reviewerRepository.GetByUserIdAsync(invitation.UserId.Value);

            if (reviewer == null)
            {
                var personResponse = await _personClient.GetPersonByUserIdAsync(new GetPersonByIdRequest() { UserId = invitation.UserId.Value });
                reviewer = Reviewer.Create(personResponse.PersonInfo, new HashSet<int> { article.JournalId }, command);
                await _reviewerRepository.AddAsync(reviewer, cancellationToken);
            }
            else
            {
                reviewer = await _reviewerRepository.GetByEmailAsync(invitation.Email, cancellationToken);
                if (reviewer is  null)
                {
                    //TODO: Implement Create User grpc;
                }
            }
        }

        invitation.Status = Domain.Invitations.Enums.InvitationStatus.Accepted;
        article.AssignReviewer(reviewer, command);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new AcceptInvitationResponse(article.Id, invitation.Id, reviewer.Id);
    }
}
