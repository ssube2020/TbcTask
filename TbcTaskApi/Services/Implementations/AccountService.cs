using AutoMapper;
using Core.Contracts.Repositories;
using Core.Contracts.Services;
using Core.Models.Entities;
using Microsoft.Extensions.Localization;
using TbcTaskApi.Dtos.RequestDtos;
using TbcTaskApi.Dtos.ResponseDtos;
using TbcTaskApi.Shared.ValidationResponseMessages;

namespace TbcTaskApi.Services;

public class AccountService(IStringLocalizerFactory localizerFactory, IMapper mapper, IUnitOfWork unitOfWork)
    : IAccountService
{
    private readonly IStringLocalizer localizer = localizerFactory.Create("ValidationResponseMessages.ResponseMessages", "TbcTaskApi");

    public async Task<GetOperationResult<List<UserResponseDto>>> GetUsersAsync(UserFilterDto filter)
    {
        var users = await unitOfWork.accountRepository.GetUsersAsync(filter.Name, filter.Surname,
            filter.PersonalNumber, filter.PageNumber, filter.PageSize);

        return new GetOperationResult<List<UserResponseDto>>()
        {
            Success = true,
            Data = mapper.Map<List<UserResponseDto>>(users)
        };
    }

    public async Task<GetOperationResult<UserResponseDto>> GetUserAsync(int userId)
    {
        var user = await unitOfWork.accountRepository.GetUserAsync(userId);

        if (user != null)
        {
            return new GetOperationResult<UserResponseDto>()
            {
                Success = true,
                Data = mapper.Map<UserResponseDto>(user)
            };
        }
        
        return new GetOperationResult<UserResponseDto>()
        {
            Success = false,
            Message =  localizer["UserNotFoundMessage"]
        };
    }

    public async Task<CreateUserResponseDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        if (await unitOfWork.accountRepository.CheckIfUserByPersonalNumber(createUserDto.PersonalNumber))
        {
            return new CreateUserResponseDto
            {
                Success = false,
                ErrorMessage = localizer["UserAlreadyExistsMessage"]
            };
        }

        if (createUserDto.Connections != null && !await CheckConnectedPeopleExist(createUserDto.Connections))
        {
            return new CreateUserResponseDto
            {
                Success = false,
                ErrorMessage = localizer["UserAlreadyExistsMessage"]
            };
        }

        var userEntity = new User
        {
            Name = createUserDto.Name,
            Surname = createUserDto.Surname,
            Gender = createUserDto.Gender,
            PersonalNumber = createUserDto.PersonalNumber,
            BirthDate = createUserDto.BirthDate,
            City = createUserDto.City,
            PhoneNumbers = mapper.Map<List<Phone>>(createUserDto.PhoneNumbers),
            Connections = mapper.Map<List<ConnectedPerson>>(createUserDto.Connections)
        };

        await unitOfWork.accountRepository.AddUserAsync(userEntity);
        await unitOfWork.Commit();

        return new CreateUserResponseDto
        {
            Success = true,
            UserId = userEntity.Id
        };
    }

    public async Task<EditUserResponseDto> EditUserInformationAsync(EditUserDto editUserDto)
    {
        if (!await unitOfWork.accountRepository.CheckUserByIdAsync(editUserDto.Id))
        {
            return new EditUserResponseDto
            {
                Success = false,
                ErrorMessage = localizer["UserNotFoundMessage"]
            };
        }

        await unitOfWork.accountRepository.EditUserAsync(mapper.Map<User>(editUserDto));
        await unitOfWork.Commit();
        
        return new EditUserResponseDto
        {
            Success = true,
            UserId = editUserDto.Id
        };
    }

    public async Task<OperationResponseDto> UploadFileAsync(UploadUserPhotoDto uploadModel)
    {
        if (!await unitOfWork.accountRepository.CheckUserByIdAsync(uploadModel.UserId))
        {
            return new OperationResponseDto
            {
                Success = false,
                ErrorMessage = localizer["UserNotFoundMessage"]
            };
        }
        
        var resultPhotoUrl = await SavePhotoLocallyAsync(uploadModel.Photo, "Content", "TbcTaskApi");

        if (!string.IsNullOrEmpty(resultPhotoUrl))
        {
            await unitOfWork.accountRepository.SavePhotoAsync(uploadModel.UserId, resultPhotoUrl);
            await unitOfWork.Commit();
            return new OperationResponseDto
            {
                Success = true,
                ErrorMessage = localizer["PhotoUploadedSuccess"]
            };
        }
        
        return new OperationResponseDto
        {
            Success = false,
            ErrorMessage = localizer["PhotoSaveError"]
        };
    }

    public async Task<OperationResponseDto> DeleteUserAsync(int userId)
    {
        if (!await unitOfWork.accountRepository.CheckUserByIdAsync(userId))
        {
            return new OperationResponseDto
            {
                Success = false,
                ErrorMessage = localizer["UserNotFoundMessage"]
            };
        }

        await unitOfWork.accountRepository.DeleteUserAsync(userId);
        await unitOfWork.Commit();
        
        return new OperationResponseDto
        {
            Success = true,
            Message = localizer["UserDeleteSuccess"]
        };
    }


    private async Task<bool> CheckConnectedPeopleExist(List<ConnectedPersonDto> connections)
    {
        if (connections != null)
        {
            var connectionsIds = connections
                .Select(c => c.ConnectedPersonId)
                .ToList();
    
            var connectionsCountFromDb = await unitOfWork.accountRepository.GetUserIdsCountByIdsAsync(connectionsIds);

            return connectionsCountFromDb == connectionsIds.Count;
        }

        return true;
    }
    
    public async Task<string> SavePhotoLocallyAsync(IFormFile photo, string rootPath, string baseUrl)
    {
        if (photo == null || photo.Length == 0)
        {
            throw new ArgumentException("Invalid photo file");
        }
        
        var photoDirectory = Path.Combine(rootPath, "Uploads", "Photos");
        if (!Directory.Exists(photoDirectory))
        {
            Directory.CreateDirectory(photoDirectory);
        }
        
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
        var filePath = Path.Combine(photoDirectory, fileName);
        
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await photo.CopyToAsync(fileStream);
        }
        
        var photoUrl = $"{baseUrl}/uploads/photos/{fileName}";
        
        return photoUrl;
    }
}