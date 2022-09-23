using Application.Common.Utilities;
using Application.Features.Auth.Dtos;
using Application.Repositories.EntityFramework;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Auth.Commands.CreateUserCommand;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserDto>
{
    private readonly IUserRepository _userRepository;
    private ITokenHelper _tokenHelper;
    private CreateUserCommandValidator _rules;
    public CreateUserCommandHandler(IUserRepository userRepository, ITokenHelper tokenHelper, CreateUserCommandValidator rules)
    {
        _userRepository = userRepository;
        _tokenHelper = tokenHelper;
    }

    public async Task<CreatedUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var findedData =await _userRepository.GetAsync(p=>p.Email == request.Email);

        HashingHelper.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
        User user = new()
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            AuthenticatorType = AuthenticatorType.None,
            Status = true,
        };

        var createdUser = await _userRepository.AddAsync(user);
        var token = _tokenHelper.CreateToken(createdUser, new List<OperationClaim>());

        return new CreatedUserDto
        {
            Expiration = token.Expiration,
            Token = token.Token
        };
    }
}
