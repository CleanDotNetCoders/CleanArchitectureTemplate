using Application.Features.Auth.Dtos;
using MediatR;

namespace Application.Features.Auth.Commands;

public class CreateUserCommand:IRequest<CreatedUserDto>
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
