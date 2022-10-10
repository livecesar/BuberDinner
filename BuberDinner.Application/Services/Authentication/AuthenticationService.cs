using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using FluentResults;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepo;
    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepo)
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

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        // Check if user already exists
        if(_userRepo.GetUserByEmail(email) is not null)
            return Errors.User.DuplicateEmail;

        // Create user: generate unic id
        var user = new User{
            FirstName = firstName,
            LastName =lastName,
            Email = email,
            Password = password
        };
        
        _userRepo.Add(user);

        // Create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}