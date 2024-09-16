using Core.Contracts.Services;
using Core.Models.Constants;
using Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using TbcTaskApi.Dtos.RequestDtos;
using TbcTaskApi.Dtos.ResponseDtos;

namespace TbcTaskApi.Controllers;

[ApiController]
[Route(RoutingConstants.Controller)]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet(RoutingConstants.Action)]
    public async Task<GetOperationResult<List<UserResponseDto>>> GetUsersFiltered([FromQuery] UserFilterDto filterDto)
        => await accountService.GetUsersAsync(filterDto);
    
    [HttpGet(RoutingConstants.Action)]
    public async Task<GetOperationResult<UserResponseDto>> GetUserById(int userId)
        => await accountService.GetUserAsync(userId);
    
    [HttpPost(RoutingConstants.Action)]
    public async Task<CreateUserResponseDto> CreateUser(CreateUserDto createDto)
        => await accountService.CreateUserAsync(createDto);
    
    [HttpPost(RoutingConstants.Action)]
    public async Task<OperationResponseDto> UploadPhoto(UploadUserPhotoDto uploadPhotoDto)
        => await accountService.UploadFileAsync(uploadPhotoDto);
    
    [HttpPut(RoutingConstants.Action)]
    public async Task<EditUserResponseDto> EditUserInformation(EditUserDto editDto)
        => await accountService.EditUserInformationAsync(editDto);
    
    [HttpDelete(RoutingConstants.Action)]
    public async Task<OperationResponseDto> DeleteUser(int userId)
        => await accountService.DeleteUserAsync(userId);
}