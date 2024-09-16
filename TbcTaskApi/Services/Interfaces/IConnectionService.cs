using Core.Models.Entities;
using TbcTaskApi.Dtos.RequestDtos;
using TbcTaskApi.Dtos.ResponseDtos;

namespace Core.Contracts.Services;

public interface IConnectionService
{
    Task<GetOperationResult<List<ConnectionsReportDto>>> GetConnectionsReportAsync(ConnectionType addConnectionDto);
    Task<OperationResponseDto> AddConnectionAsync(AddConnectedPersonDto addConnectionDto);
    Task<OperationResponseDto> DeleteConnectionUserAsync(DeleteConnectedPersonDto deleteConnDto);
}
