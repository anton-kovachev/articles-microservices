using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using Blocks.Exceptions;
using Blocks.AspNetCore.Extensions;
using Auth.Application;
using Microsoft.AspNetCore.Authorization;
using Auth.Persistence.Repositories;

namespace Auth.API.Features.Users.Login
{
    [AllowAnonymous]
    [HttpPost("login")]
    public class LoginEndpoint(PersonRepository _personRepository, UserManager<User> _userManager, SignInManager<User> _signInManager, TokenFactory _tokenFactory) : Endpoint<LoginRequest, LoginResponse>
    {
        public override async Task HandleAsync(LoginRequest command, CancellationToken cancellationToken)
        {
            var person = Guard.NotFound(await _personRepository.GetByEmailAsync(command.Email));
            var user = Guard.NotFound(person.User);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
                throw new BadRequestException($"Invalid credentials for {command.Email}.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var jwtToken = _tokenFactory.GenerateJWTToken(user.Id.ToString(), user.Person.FullName, user.Email, userRoles, Enumerable.Empty<Claim>());
            var refreshToken = _tokenFactory.GenerateRefreshToken(HttpContext.GetClientIpAddress());
            user.AddRefreshToken(refreshToken);

            await Send.OkAsync(new LoginResponse(user.Email, jwtToken, refreshToken.Token));
        }
    }
}
