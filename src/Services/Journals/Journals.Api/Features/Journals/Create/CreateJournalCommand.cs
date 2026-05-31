using Blocks.Core.Constraints;
using Blocks.Core.FluentValidation;
using FastEndpoints;
using FluentValidation;

namespace Journals.Api.Features.Journals.Create;

public record CreateJournalCommand(string Name, string Abbreviation, string Description, string ISSN, int ChiefEditorId)
{
}

public class CreateJournalCommandValidator : Validator<CreateJournalCommand>
{ 
    public CreateJournalCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLengthWithMessage(maxLength: MaxLength.C64, nameof(CreateJournalCommand.Name)); 
        RuleFor(x => x.Abbreviation).NotEmpty().MaximumLengthWithMessage(maxLength: MaxLength.C8, nameof(CreateJournalCommand.Abbreviation)); 
        RuleFor(x => x.Description).NotEmpty().MaximumLengthWithMessage(maxLength: MaxLength.C2048, nameof(CreateJournalCommand.Description));
        RuleFor(x => x.ISSN).NotEmpty().Matches(@"\d{4}-\d{3}[\dX]").WithMessage("Invalid ISSN format"); 
        RuleFor(x => x.ChiefEditorId).GreaterThan(0).WithMessageForInvalidId(nameof(CreateJournalCommand.ChiefEditorId)); 
    }
}

