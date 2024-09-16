namespace TbcTaskApi.Shared.Exceptions;

public class ErrorModel
{
    public string Path { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();
    public int HttpStatusCode { get; set; }
}