using Core.Models.Entities;
using TbcTaskApi.Dtos.RequestDtos;
using TbcTaskApi.Dtos.ResponseDtos;

namespace Core.Contracts.Services;

public interface IAccountService
{
    Task<GetOperationResult<List<UserResponseDto>>> GetUsersAsync(UserFilterDto filter);
    Task<GetOperationResult<UserResponseDto>> GetUserAsync(int userId);
    Task<CreateUserResponseDto> CreateUserAsync(CreateUserDto dto);
    Task<EditUserResponseDto> EditUserInformationAsync(EditUserDto dto);
    Task<OperationResponseDto> UploadFileAsync(UploadUserPhotoDto photoUploadDto);
    Task<OperationResponseDto> DeleteUserAsync(int userId);
}