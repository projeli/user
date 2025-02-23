using AutoMapper;
using UserService.Application.Dtos;
using UserService.Domain.Models;

namespace UserService.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ProjeliUser, ProjeliUserDto>();
    }
}