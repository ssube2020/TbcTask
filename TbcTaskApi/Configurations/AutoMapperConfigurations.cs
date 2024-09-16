using AutoMapper;
using Core.Models.Entities;
using TbcTaskApi.Dtos.RequestDtos;
using TbcTaskApi.Dtos.ResponseDtos;

namespace TbcTaskApi.Configurations;

public static class AutoMapperConfigurations
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddSingleton(GetMapper());
    }

    private static IMapper GetMapper()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.CreateMap<PhoneRequestDto, Phone>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            
            mc.CreateMap<ConnectedPersonDto, ConnectedPerson>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            mc.CreateMap<AddConnectedPersonDto, ConnectedPerson>();
            
            mc.CreateMap<EditUserDto, User>();
            
            mc.CreateMap<EditUserDto, User>()
                .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom((src, dest, destMember, context) => 
                {
                    return src.PhoneNumbers?.Select(phoneRequestDto => new Phone
                    {
                        PhoneNumber = phoneRequestDto.PhoneNumber,
                        PhoneNumberType = phoneRequestDto.PhoneNumberType
                    }).ToList();
                }));

            mc.CreateMap<User, UserResponseDto>();
            
            mc.CreateMap<Phone, PhoneDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
            
            mc.CreateMap<ConnectedPerson, ConnectionDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ConnectedPersonId, opt => opt.MapFrom(src => src.ConnectedPersonId))
                .ReverseMap();
            
            mc.CreateMap<User, ConnectionsReportDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ConnectedPeopleCount, opt => opt.MapFrom(src => src.Connections.Count))
                .ReverseMap();
        });

        return mapperConfig.CreateMapper();
    }
}