using FastEndpoints;
using FluentValidation;

namespace Auth.API.Features.Users.CreateUser;

public class CreateUserCommandValidator : Validator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.UserRoles).NotEmpty().Must((c, roles) => AreUserRoleDateValid(roles)).WithMessage("Invalid roles");
    }

    public static bool AreUserRoleDateValid(IReadOnlyList<UserRoleDto> roles)
        // Start date must be today or in the future
        //Expiration date must be after start date or today if no start date is specified
        => roles.All(role => !role.StartDate.HasValue || role.StartDate.Value >= DateTime.UtcNow) &&
                roles.All(role => !role.ExpiringDate.HasValue || role.ExpiringDate.Value >= (role.StartDate ?? DateTime.UtcNow));

}
