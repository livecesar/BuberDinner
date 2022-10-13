using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepo;

    public RegisterCommandHandler(IUserRepository userRepo, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepo = userRepo;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
          // Check if user already exists
        if(_userRepo.GetUserByEmail(command.Email) is not null)
            return Errors.User.DuplicateEmail;

        // Create user: generate unic id
        var user = new User{
            FirstName = command.FirstName,
            LastName =command.LastName,
            Email = command.Email,
            Password = command.Password
        };
        
        _userRepo.Add(user);

        // Create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}