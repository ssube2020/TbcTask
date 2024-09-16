using Core.Models.Entities;

namespace TbcTaskApi.Dtos.RequestDtos;

public class PhoneRequestDto
{
    public PhoneNumberType PhoneNumberType { get; set; }
    public string PhoneNumber { get; set; }
}