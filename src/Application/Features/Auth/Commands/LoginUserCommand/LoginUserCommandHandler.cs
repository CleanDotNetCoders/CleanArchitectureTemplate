using Application.Common.Exceptions;
using Application.Common.Utilities;
using Application.Features.Auth.Dtos;
using Application.Features.Auth.Models;
using Application.Repositories.EntityFramework;
using Application.Services;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.LoginUserCommand;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginedUserDto>
{
    private readonly IUserRepository _userRepository;
    private IMapper _mapper;
    private ITokenHelper _tokenHelper;

    public LoginUserCommandHandler(IUserRepository repository,
        ITokenHelper tokenHelper, IMapper mapper)
    {
        _userRepository = repository;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
    }

    public async Task<LoginedUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(
            u => u.Email.ToLower() == request.Email.ToLower(),
            include: m => m.Include(c => c.UserOperationClaims).ThenInclude(x => x.OperationClaim));

        if (user == null)
            throw new BusinessException("This user doesn't exist");

        var operationClaims = user.UserOperationClaims.Select(x => x.OperationClaim).ToList();

        var result = HashingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
        if (!result)
            throw new BusinessException("Wrong credentials");

        AccessToken token = _tokenHelper.CreateToken(user, operationClaims);

        LoginedUserDto loginedUserDto = _mapper.Map<LoginedUserDto>(token);
        return loginedUserDto;
    }
}