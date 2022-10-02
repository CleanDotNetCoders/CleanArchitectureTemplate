using Application.Common.Utilities;
using Application.Features.Auth.Dtos;
using Application.Features.Auth.Rules;
using Application.Repositories.EntityFramework;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands.CreateUserCommand;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private ITokenHelper _tokenHelper;
    private IHttpContextAccessor _accessor;
    private IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, ITokenHelper tokenHelper, IHttpContextAccessor accessor, IMapper mapper, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _tokenHelper = tokenHelper;
        _accessor = accessor;
        _mapper = mapper;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<CreatedUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var findedData =await _userRepository.GetAsync(p=>p.Email == request.Email);

        var remoteIpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

        AuthBusinessRules.BeUniqueEmail(findedData);
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
        var token = _tokenHelper.CreateRefreshToken(createdUser, remoteIpAddress);
        await _refreshTokenRepository.AddAsync(token);
        var mappedData = _mapper.Map<CreatedUserDto>(token);
        return mappedData;
    }
}
