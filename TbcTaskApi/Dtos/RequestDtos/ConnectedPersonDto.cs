using Core.Models.Entities;

namespace TbcTaskApi.Dtos.RequestDtos;

public class ConnectedPersonDto
{
    public ConnectionType ConnectionType { get; set; }
    public int ConnectedPersonId { get; set; }
}