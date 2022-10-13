using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using FluentResults;

namespace BuberDinner.Application.Services.Authentication.Queries;

public class AuthenticationQueryService : IAuthenticationQueryService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepo;
    public AuthenticationQueryService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepo)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepo = userRepo;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        // Validate the user exists
        if(_userRepo.GetUserByEmail(email) is not User user)
            return Errors.Authentication.InvalidCredentials;

        // Validate the password is correct
        if(!user.Password.Equals(password))
            return Errors.Authentication.InvalidCredentials;

        // Create JWT
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }

}