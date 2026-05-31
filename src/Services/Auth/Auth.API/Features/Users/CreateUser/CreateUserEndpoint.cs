using Articles.Security;
using Auth.Domain.Persons;
using Auth.Domain.Users.Events;
using Auth.Persistence;
using Auth.Persistence.Repositories;
using Blocks.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Features.Users.CreateUser
{
    [Authorize(Roles = Role.USERADMIN)]
    [HttpPost("users")]
    public class CreateUserEndpoint(PersonRepository _personRepository, AuthDbContext _dbContext, UserManager<User> _userManager) : Endpoint<CreateUserCommand, CreateUserResponse>
    {
        public override async Task HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByEmailAsync(command.Email);
            if (person?.User is not null)
                throw new BadRequestException($"User with email {command.Email} already exists.");

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            if (person is null)
                person = await CreatePerson(command, cancellationToken);

            var user = Domain.Users.User.Create(command);
            person.AssignUser(user);
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(" | ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new BadRequestException(errorMessages);
            }

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _personRepository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            await PublishAsync(new UserCreated(user, resetPasswordToken));
            await Send.OkAsync(new CreateUserResponse(Email: user.Email!, UserId: user.Id, Token: resetPasswordToken));
        }

        private async Task<Person> CreatePerson(CreateUserCommand command, CancellationToken cancellationToken)
        {
            Person person = Person.Create(command);
            await _personRepository.AddAsync(person);
            await _personRepository.SaveChangesAsync(cancellationToken);

            return person;
        }
    }
}
