using Core.Models.Entities;

namespace TbcTaskApi.Dtos.ResponseDtos;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public GenderEnum Gender { get; set; }
    public string PersonalNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string City { get; set; }
    public string? PhotoUrl { get; set; }
    public List<PhoneDto> PhoneNumbers { get; set; }
    public List<ConnectionDto> Connections { get; set; }
}

public class PhoneDto
{
    public int Id { get; set; }
    public PhoneNumberType PhoneNumberType { get; set; }
    public string PhoneNumber { get; set; }
    public int UserId { get; set; }
}

public class ConnectionDto
{
    public int Id { get; set; }
    public ConnectionType ConnectionType { get; set; }
    public int ConnectedPersonId { get; set; }
    public int UserId { get; set; }
}
