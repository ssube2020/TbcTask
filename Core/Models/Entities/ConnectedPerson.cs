namespace Core.Models.Entities;

public class ConnectedPerson : EntityBase
{
    public ConnectionType ConnectionType { get; set; }
    public int ConnectedPersonId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
