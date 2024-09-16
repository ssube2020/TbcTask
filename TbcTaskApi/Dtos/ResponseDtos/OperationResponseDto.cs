namespace TbcTaskApi.Dtos.ResponseDtos;

public class OperationResponseDto
{
    public bool Success { get; set; } = false;
    public string? ErrorMessage { get; set; } = null;
    public string? Message { get; set; } = null;
}