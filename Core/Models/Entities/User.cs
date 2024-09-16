using System.Runtime.InteropServices.JavaScript;

namespace Core.Models.Entities;

public class User : EntityBase
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public GenderEnum Gender { get; set; }
    public string PersonalNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string City { get; set; }
    public string? PhotoUrl { get; set; }
    public List<Phone> PhoneNumbers { get; set; }
    public List<ConnectedPerson> Connections { get; set; }
}

