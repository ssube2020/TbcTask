using Core.Models.Entities;

namespace TbcTaskApi.Dtos.RequestDtos;

public class CreateUserDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public GenderEnum Gender { get; set; }
    public string PersonalNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string City { get; set; }
    
    public string? PhotoUrl { get; set; }
    public List<PhoneRequestDto>? PhoneNumbers { get; set; }
    public List<ConnectedPersonDto>? Connections { get; set; }
}