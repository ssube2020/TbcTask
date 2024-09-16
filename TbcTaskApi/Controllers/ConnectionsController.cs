using Core.Contracts.Services;
using Core.Models.Constants;
using Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using TbcTaskApi.Dtos.RequestDtos;
using TbcTaskApi.Dtos.ResponseDtos;

namespace TbcTaskApi.Controllers;

[ApiController]
[Route(RoutingConstants.Controller)]
public class ConnectionsController(IConnectionService connectionService) : ControllerBase
{
    [HttpGet(RoutingConstants.Action)]
    public async Task<GetOperationResult<List<ConnectionsReportDto>>> GetConnectionsReport(ConnectionType connectionType)
        => await connectionService.GetConnectionsReportAsync(connectionType);
    
    [HttpPost(RoutingConstants.Action)]
    public async Task<OperationResponseDto> AddConnectionToUser(AddConnectedPersonDto addConnectionDto)
        => await connectionService.AddConnectionAsync(addConnectionDto);
    
    [HttpDelete(RoutingConstants.Action)]
    public async Task<OperationResponseDto> DeleteConnectedUser(DeleteConnectedPersonDto deleteConnectionDto)
        => await connectionService.DeleteConnectionUserAsync(deleteConnectionDto);
}