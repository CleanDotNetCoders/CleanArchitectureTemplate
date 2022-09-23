using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Behaviours.Authorization;
using Application.Common.Behaviours.Validation;
using Application.Features.Auth.Commands.CreateUserCommand;
using Application.Features.Auth.Commands.LoginUserCommand;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<CreateUserCommandValidator>();
        services.AddScoped<LoginUserCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        return services;
    }
}