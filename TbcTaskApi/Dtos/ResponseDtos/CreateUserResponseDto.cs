namespace TbcTaskApi.Dtos.ResponseDtos;

public class CreateUserResponseDto
{
    public bool Success { get; set; } = false;
    public int? UserId { get; set; } = null;
    public string? ErrorMessage { get; set; } = null;
}