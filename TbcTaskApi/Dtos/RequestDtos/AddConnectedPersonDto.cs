using Core.Models.Entities;

namespace TbcTaskApi.Dtos.RequestDtos;

public class AddConnectedPersonDto
{
    public int UserId { get; set; }
    public int ConnectedPersonId { get; set; }
    public ConnectionType ConnectionType { get; set; }
}