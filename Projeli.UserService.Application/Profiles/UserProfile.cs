using AutoMapper;
using Projeli.UserService.Application.Dtos;
using Projeli.UserService.Application.Models.Requests;
using Projeli.UserService.Application.Models.Responses;
using Projeli.UserService.Domain.Models;

namespace Projeli.UserService.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();

        CreateMap<UserDto, UserResponse>();
        
        CreateMap<CreateUserRequest, UserDto>();
        CreateMap<UpdateUserRequest, UserDto>();
    }
}