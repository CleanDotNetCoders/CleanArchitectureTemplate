using Application.Common.Utilities;
using Application.Features.Auth.Dtos;
using Application.Repositories.EntityFramework;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.LoginUserCommand
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginedUserDto>
    {
        private readonly IUserRepository _repository;
        private LoginUserCommandValidator _validator;
        private IMapper _mapper;
        private ITokenHelper _tokenHelper;
        public LoginUserCommandHandler(LoginUserCommandValidator validator, IUserRepository repository, ITokenHelper tokenHelper, IMapper mapper)
        {
            _validator = validator;
            _repository = repository;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
        }

        public async Task<LoginedUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user =await _repository.GetAsync(p=>p.Email == request.Email);
            await _validator.CheckEmailAndPassword(request.Password, user);

            var token = _tokenHelper.CreateToken(user, new List<OperationClaim>() { });
            return _mapper.Map<LoginedUserDto>(token);

        }
    }
}
