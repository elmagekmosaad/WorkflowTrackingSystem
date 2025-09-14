using AutoMapper;
using WorkflowTrackingSystem.Api.Requests.Process;
using WorkflowTrackingSystem.Api.Requests.Workflow;
using WorkflowTrackingSystem.Application.DTOs.Process;
using WorkflowTrackingSystem.Application.DTOs.Workflow;
using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Api.Mapping
{
    public class ProcessProfile : Profile
    {
        public ProcessProfile()
        {
            CreateMap<Process, ProcessDto>()
                .ReverseMap();

            CreateMap<StartProcessRequest, ProcessDto>()
                .ReverseMap();




            CreateMap<ExecuteStepDto, ProcessDto>()
                .ReverseMap();

            CreateMap<ExecuteProcessRequest, ExecuteStepDto>()
                .ReverseMap();
        }
      
     
    }

   
}
