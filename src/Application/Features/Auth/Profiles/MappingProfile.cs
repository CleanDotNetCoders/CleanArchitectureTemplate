using Application.Features.Auth.Commands.CreateUserCommand;
using Application.Features.Auth.Commands.LoginUserCommand;
using Application.Features.Auth.Dtos;
using Application.Features.Auth.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserCommand,CreatedUserDto>().ReverseMap();
            CreateMap<LoginUserCommand,LoginedUserDto>().ReverseMap();

            CreateMap<AccessToken, LoginedUserDto>().ReverseMap();
        }
    }
}
