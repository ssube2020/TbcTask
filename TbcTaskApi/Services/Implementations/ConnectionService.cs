using AutoMapper;
using Core.Contracts.Repositories;
using Core.Contracts.Services;
using Core.Models.Entities;
using Microsoft.Extensions.Localization;
using TbcTaskApi.Dtos.RequestDtos;
using TbcTaskApi.Dtos.ResponseDtos;

namespace TbcTaskApi.Services;

public class ConnectionService(IMapper mapper, IUnitOfWork unitOfWork, IStringLocalizerFactory localizerFactory) : IConnectionService
{
    private readonly IStringLocalizer localizer = localizerFactory.Create("ValidationResponseMessages.ResponseMessages", "TbcTaskApi");
    
    public async Task<GetOperationResult<List<ConnectionsReportDto>>> GetConnectionsReportAsync(ConnectionType addConnectionDto)
    {
        var databaseResult = await unitOfWork.connectionRepository.GetConnectionsReportByType(addConnectionDto);
        
        var userDtos = mapper.Map<List<ConnectionsReportDto>>(databaseResult);

        return new GetOperationResult<List<ConnectionsReportDto>>
        {
            Success = true,
            Data = userDtos
        };
    }

    public async Task<OperationResponseDto> AddConnectionAsync(AddConnectedPersonDto addConnectionDto)
    {
        if (!await unitOfWork.accountRepository.CheckUserByIdAsync(addConnectionDto.UserId)
            || !await unitOfWork.accountRepository.CheckUserByIdAsync(addConnectionDto.ConnectedPersonId))
        {
            return new OperationResponseDto
            {
                Success = false,
                ErrorMessage = localizer["UserNotFoundMessage"]
            };
        }
        
        if (await unitOfWork.connectionRepository.ConnectionExistsAsync(addConnectionDto.UserId, addConnectionDto.ConnectedPersonId))
        {
            return new OperationResponseDto
            {
                Success = false,
                ErrorMessage = localizer["ConnectionExists"]
            };
        }

        var connectionToAdd = mapper.Map<ConnectedPerson>(addConnectionDto);
        
        await unitOfWork.connectionRepository.AddConnectionAsync(connectionToAdd);
        await unitOfWork.Commit();
        
        return new OperationResponseDto
        {
            Success = false,
            ErrorMessage = localizer["ConnectionCreatedSuccess"]
        };
    }
    
    public async Task<OperationResponseDto> DeleteConnectionUserAsync(DeleteConnectedPersonDto deleteConnDto)
    {
        if (!await unitOfWork.accountRepository.CheckUserByIdAsync(deleteConnDto.UserId)
            || !await unitOfWork.accountRepository.CheckUserByIdAsync(deleteConnDto.ConnectedUserId))
        {
            return new OperationResponseDto
            {
                Success = false,
                ErrorMessage = localizer["UserNotFoundMessage"]
            };
        }

        await unitOfWork.accountRepository.DeleteConnectedUserAsync(deleteConnDto.UserId, deleteConnDto.ConnectedUserId);
        await unitOfWork.Commit();
        
        return new OperationResponseDto
        {
            Success = true,
            Message = localizer["UserDeleteSuccess"]
        };
    }
}