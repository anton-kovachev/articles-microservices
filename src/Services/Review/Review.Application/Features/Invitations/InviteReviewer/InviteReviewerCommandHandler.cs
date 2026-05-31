using Auth.Grpc;
using EmailService.Contract;
using Flurl;
using MediatR;
using Microsoft.Extensions.Options;
using Review.Application.Options;
using Review.Domain.Articles;
using Review.Domain.Invitations;

namespace Review.Application.Features.Invitations.InviteReviewer;

public class InviteReviewerCommandHandler(
    ReviewDbContext _reviewDbContext,
    ArticleRepository _articleRepository,
    ReviewerRepository _reviewerRepository,
    IPersonService _personService,
    IEmailService _emailService,
    IOptions<AppUrlsOptions> _appUrlOptions,
    IOptions<EmailOptions> _emailOptions)
    : IRequestHandler<InviteReviewerCommand, InviteReviewerResponse>
{
    public async Task<InviteReviewerResponse> Handle(InviteReviewerCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdOrThrowAsync(command.ArticleId);

        ReviewInvitation invitation = default!;
        if (command.UserId is not null)
        {
            var reviewer = await _reviewerRepository.GetByUserIdAsync(command.UserId.Value);

            if (reviewer is not null)
            {
                invitation = article.InviteReviewer(reviewer, command);
            }
            else
            {
                var response = await _personService.GetPersonByUserIdAsync(
                    new GetPersonByIdRequest { UserId = command.UserId.Value });

                var personInfo = response.PersonInfo;
                invitation = article.InviteReviewer(personInfo.UserId, personInfo.Email, personInfo.FirstName, personInfo.LastName, command);
            }
        }
        else
        {
            invitation = article.InviteReviewer(command.UserId, command.Email, command.FirstName, command.LastName, command);
        }

        await _reviewDbContext.SaveChangesAsync(cancellationToken);

        var editor = await _reviewDbContext.Editors.SingleAsync(e => e.UserId == command.CreatedById, cancellationToken);
        await _emailService.SendEmailAsync(BuildEmalMessage(article, invitation, editor), cancellationToken);

        return new InviteReviewerResponse(invitation.ArticleId, invitation.Id, invitation.Token.Value);
    }

    private EmailMessage BuildEmalMessage(Article article, ReviewInvitation invitation, Editor editor)
    {
        const string InvitationEmail =
            @"Dear Contributor, <br/>
               You've been invited by {0} to review the following article: {1}. <br/>
               Please accept or deny, the invitation will expire on {2}. <br/>
               If you don't have any account please create one using the following URL: {3}";

        var url = _appUrlOptions.Value.ReviewUIBaseUrl
            .AppendPathSegment($"articles/{article.Id}/invitations/{invitation.Token}");

        return new EmailMessage("Review invitation", new EmailContent(
            ContentType.Html,
            string.Format(InvitationEmail, editor.FullName, article.Title, invitation.ExpiresOn, url)),
            new EmailAddress("articles", _emailOptions.Value.EmailFromAddress),
            new List<EmailAddress> { new EmailAddress(invitation.FullName, invitation.Email) });
    }
}
