namespace TbcTaskApi.Dtos.RequestDtos;

public class UploadUserPhotoDto
{
    public int UserId { get; set; }
    public IFormFile Photo { get; set; }
}