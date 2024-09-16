namespace Core.Models.Entities;
public class Phone : EntityBase
{
    public PhoneNumberType PhoneNumberType { get; set; }
    public string PhoneNumber { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
