using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcessTrackingSystem.Application.Services.Interfaces;
using WorkflowTrackingSystem.Api.Requests.Process;
using WorkflowTrackingSystem.Application.DTOs.Process;
using WorkflowTrackingSystem.Domain.Enums;
using WorkflowTrackingSystem.Shared;

namespace ProcessTrackingSystem.Api.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProcessesController : ControllerBase
    {
        private readonly IProcessService _processService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProcessesController> _logger;
        public ProcessesController(IProcessService processService, IMapper mapper, ILogger<ProcessesController> logger)
        {
            _processService = processService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("start")]
        [ProducesResponseType(typeof(BaseResponse<ProcessDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> StartProcess([FromBody] StartProcessRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for CreateProcessRequest: {ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                if (request.Initiator == null)
                {
                    _logger.LogWarning("CreateProcessRequest must contain an initiator.");
                    return BadRequest("Process must contain an initiator.");
                }

                if (request.WorkflowId == Guid.Empty)
                {
                    _logger.LogWarning("CreateProcessRequest must contain a valid WorkflowId.");
                    return BadRequest("Process must contain a valid WorkflowId.");
                }

                _logger.LogInformation("Creating new process with Initiator: {Initiator}", request.Initiator);

                var processDto = _mapper.Map<ProcessDto>(request);
                _logger.LogDebug("Mapped CreateProcessRequest to CreateProcessDto: {@ProcessDto}", processDto);

                var process = await _processService.StartProcessAsync(processDto);
                if (process != null)
                {
                    _logger.LogInformation("Process started successfully with ID: {ProcessId}", process?.Data?.Id);
                    return Ok(process);
                }
                else
                {
                    _logger.LogWarning("Failed to start process for Initiator: {Initiator}", request.Initiator);
                    return BadRequest("Failed to start process.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating process.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("execute")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ExecuteStep([FromBody] ExecuteProcessRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for ExecuteProcessRequest: {ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                if (request.ProcessId == Guid.Empty || string.IsNullOrWhiteSpace(request.StepName) || string.IsNullOrWhiteSpace(request.PerformedBy) || string.IsNullOrWhiteSpace(request.Action))
                {
                    _logger.LogWarning("ExecuteProcessRequest contains invalid data.");
                    return BadRequest("Invalid input data.");
                }
                _logger.LogInformation("Executing StepName: {StepName} for ProcessId: {ProcessId} PerformedBy: {PerformedBy}", request.StepName, request.ProcessId, request.PerformedBy);

                var processDto = _mapper.Map<ExecuteStepDto>(request);
                _logger.LogDebug("Mapped ExecuteProcessRequest to ExecuteStepDto: {@ExecuteStepDto}", processDto);

                var result = await _processService.ExecuteStepAsync(processDto);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Step executed successfully for ProcessId: {ProcessId}", request.ProcessId);
                    return NoContent();
                }
                else
                {
                    _logger.LogWarning("Failed to execute step for ProcessId: {ProcessId}", request.ProcessId);
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing process step.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<PaginatedList<ProcessDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseResponse<PaginatedList<ProcessDto>>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] Guid? workflowId = null, [FromQuery] ProcessStatus? status = null, [FromQuery] string? assignedTo = null)
        {
            try
            {
                _logger.LogInformation("Fetching processes with filters - PageNumber: {PageNumber}, PageSize: {PageSize}, WorkflowId: {WorkflowId}, Status: {Status}, AssignedTo: {AssignedTo}", pageNumber, pageSize, workflowId, status, assignedTo);
                
                var result = await _processService.GetAllProcesssAsync(pageNumber, pageSize, workflowId, status, assignedTo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching processs");
                return BadRequest(BaseResponse<object>.Fail("Internal server error"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProcessDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching process with ID: {ProcessId}", id);
                var process = await _processService.GetProcessByIdAsync(id);
                if (process == null)
                {
                    _logger.LogWarning("Process with ID: {ProcessId} not found.", id);
                    return NotFound();
                }
                _logger.LogInformation("Process with ID: {ProcessId} retrieved successfully.", id);
                return Ok(process);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching process with ID: {ProcessId}", id);
                return BadRequest(BaseResponse<object>.Fail("Error updating process"));
            }
        }

    }
}
