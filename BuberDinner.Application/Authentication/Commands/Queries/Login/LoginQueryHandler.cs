using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepo;

    public LoginQueryHandler(IUserRepository userRepo, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepo = userRepo;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
          // Validate the user exists
        if(_userRepo.GetUserByEmail(query.Email) is not User user)
            return Errors.Authentication.InvalidCredentials;

        // Validate the password is correct
        if(!user.Password.Equals(query.Password))
            return Errors.Authentication.InvalidCredentials;

        // Create JWT
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}