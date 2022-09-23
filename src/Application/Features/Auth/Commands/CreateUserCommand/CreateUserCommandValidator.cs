using Application.Common.Exceptions;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.CreateUserCommand
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p=>p.Email).NotEmpty().EmailAddress();
            RuleFor(p=>p.Password).NotEmpty();
            RuleFor(p=>p.FirstName).NotEmpty();
            RuleFor(p=>p.LastName).NotEmpty();
        }

        public async Task CheckIfEmailExist(User user) {
            if (user != null) throw new BusinessException("Email Exist");
        }

    }
}
