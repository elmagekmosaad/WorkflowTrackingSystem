using AutoMapper;
using WorkflowTrackingSystem.Api.Requests.Workflow;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Api.Mapping
{
    public class WorkflowProfile : Profile
    {
        public WorkflowProfile()
        {
            CreateMap<Workflow, WorkflowDto>()
                .ReverseMap();

            CreateMap<CreateWorkflowRequest, WorkflowDto>()
                .ReverseMap();

        }
     
    }

   
}
