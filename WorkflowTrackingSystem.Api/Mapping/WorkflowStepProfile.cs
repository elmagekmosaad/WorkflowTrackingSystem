using AutoMapper;
using WorkflowTrackingSystem.Api.Requests.Workflow;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Api.Mapping
{
    public class WorkflowStepProfile : Profile
    {
        public WorkflowStepProfile()
        {
            CreateMap<WorkflowStep, WorkflowStepDto>()
                .ReverseMap();

            CreateMap<CreateWorkflowStepRequest, WorkflowStepDto>()
                .ReverseMap();

            CreateMap<WorkflowStep, WorkflowStepDto>()
                .ReverseMap();

           
        }
    }

   
}
