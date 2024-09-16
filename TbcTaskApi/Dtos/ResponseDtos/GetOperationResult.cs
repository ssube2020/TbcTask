namespace TbcTaskApi.Dtos.ResponseDtos;

public class GetOperationResult<T>
{
    public bool? Success { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }
}