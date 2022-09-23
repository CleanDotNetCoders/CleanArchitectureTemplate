using Application.Common.Utilities;
using Application.Features.Auth.Dtos;
using Application.Repositories.EntityFramework;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Auth.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserDto>
{
    private readonly IUserRepository _userRepository;
    private IMapper _mapper;
    private ITokenHelper _tokenHelper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, ITokenHelper tokenHelper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenHelper = tokenHelper;
    }

    public async Task<CreatedUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        HashingHelper.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
        User user = new()
        {
            Id = request.Id,
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