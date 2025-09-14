using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkflowTrackingSystem.Api.Requests.Workflow;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Application.Services.Interfaces;
using WorkflowTrackingSystem.Shared;

namespace WorkflowTrackingSystem.Api.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;
        private readonly IMapper _mapper;
        private readonly ILogger<WorkflowsController> _logger;
        public WorkflowsController(IWorkflowService workflowService, IMapper mapper, ILogger<WorkflowsController> logger)
        {
            _workflowService = workflowService;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<PaginatedList<WorkflowDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseResponse<PaginatedList<WorkflowDto>>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Fetching all workflows with pagination. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

                var result = await _workflowService.GetAllWorkflowsAsync(pageNumber, pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching workflows");
                return BadRequest(BaseResponse<object>.Fail("Internal server error"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching workflow with ID: {WorkflowId}", id);
                var workflow = await _workflowService.GetWorkflowByIdAsync(id);
                if (workflow == null)
                {
                    _logger.LogWarning("Workflow with ID: {WorkflowId} not found.", id);
                    return NotFound();
                }
                _logger.LogInformation("Workflow with ID: {WorkflowId} retrieved successfully.", id);
                return Ok(workflow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching workflow with ID: {WorkflowId}", id);
                return BadRequest(BaseResponse<object>.Fail("Error updating workflow"));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateWorkflowRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for CreateWorkflowRequest: {ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                if (request.Steps == null || !request.Steps.Any())
                {
                    _logger.LogWarning("CreateWorkflowRequest must contain at least one step.");
                    return BadRequest("Workflow must contain at least one step.");
                }
                _logger.LogInformation("Creating new workflow with name: {WorkflowName}", request.Name);

                var workflowDto = _mapper.Map<WorkflowDto>(request);
                _logger.LogDebug("Mapped CreateWorkflowRequest to CreateWorkflowDto: {@WorkflowDto}", workflowDto);

                var workflowId = await _workflowService.CreateWorkflowAsync(workflowDto);
                _logger.LogInformation("Workflow created successfully with ID: {WorkflowId}", workflowId);
                return CreatedAtAction(nameof(GetById), new { id = workflowId }, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating workflow.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<WorkflowDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateWorkflowRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for UpdateWorkflowRequest: {ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                if (id.Equals(Guid.Empty) || request == null)
                    return BadRequest(BaseResponse<object>.Fail("Invalid input"));


                if (request.Steps == null || !request.Steps.Any())
                {
                    _logger.LogWarning("UpdateWorkflowRequest must contain at least one step.");
                    return BadRequest("Workflow must contain at least one step.");
                }

                _logger.LogInformation("Updating workflow with name: {WorkflowName}", request.Name);

                var workflowDto = _mapper.Map<WorkflowDto>(request);
                _logger.LogDebug("Mapped CreateWorkflowRequest to CreateWorkflowDto: {@WorkflowDto}", workflowDto);

                var result = await _workflowService.UpdateWorkflowAsync(id, workflowDto);
                _logger.LogInformation("Updated workflow with name: {WorkflowName}", request.Name);

                return result.Succeeded ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow");
                return BadRequest(BaseResponse<object>.Fail("Error updating workflow"));
            }
        }

    }
}
