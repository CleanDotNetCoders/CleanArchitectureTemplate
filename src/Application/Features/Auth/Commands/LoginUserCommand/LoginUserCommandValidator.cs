using Application.Common.Exceptions;
using Application.Common.Utilities;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.LoginUserCommand
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(p=>p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Password).NotEmpty();
        }

        public async Task CheckEmailAndPassword(string password, User user)
        {

            if (user == null) throw new BusinessException("email is not found");

            if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new BusinessException("Password is not match");
        }
    }
}
