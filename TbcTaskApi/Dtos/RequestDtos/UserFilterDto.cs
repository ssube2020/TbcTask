namespace TbcTaskApi.Dtos.RequestDtos;

public class UserFilterDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PersonalNumber { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
