using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WorkflowTrackingSystem.Api.Controllers.v1
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class WorkflowsController : ControllerBase
    {
    }
}
