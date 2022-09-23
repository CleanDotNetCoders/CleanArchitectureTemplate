using Application.Repositories.EntityFramework;
using FluentValidation;

namespace Application.Features.Auth.Commands.CreateUserCommand;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;


        RuleFor(p => p.Email).NotEmpty().NotNull().EmailAddress().MustAsync(BeUniqueEmail)
            .WithMessage("The specified user already exists.");
        RuleFor(p => p.Password).NotEmpty().NotNull();
        RuleFor(p => p.FirstName).NotEmpty().NotNull();
        RuleFor(p => p.LastName).NotEmpty().NotNull();
    }

    public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower());
        if (result != null)
        {
            return false;
        }
        return true;
    }
}