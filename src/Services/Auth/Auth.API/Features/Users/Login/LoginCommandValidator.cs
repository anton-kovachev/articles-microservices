using FastEndpoints;
using FluentValidation;

namespace Auth.API.Features.Users.Login
{
    public class LoginCommandValidator : Validator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
