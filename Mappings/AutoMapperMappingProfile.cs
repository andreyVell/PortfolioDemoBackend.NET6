using AutoMapper;
using DataCore.Entities;
using Services.Models._BaseModels;
using Services.Models.Authentication;
using Services.Models.AvetonRole;
using Services.Models.AvetonRoleAccess;
using Services.Models.AvetonUser;
using Services.Models.Chat;
using Services.Models.ChatMember;
using Services.Models.ChatMessage;
using Services.Models.Client;
using Services.Models.Division;
using Services.Models.DivisionContractor;
using Services.Models.Employee;
using Services.Models.Job;
using Services.Models.Organization;
using Services.Models.Person;
using Services.Models.Position;
using Services.Models.Project;
using Services.Models.ProjectStage;
using Services.Models.StageManager;
using Services.Models.StageReport;
using Services.Models.StageReportAttachedFile;
using WebApi.DTOs;
using WebApi.DTOs.Authentication;
using WebApi.DTOs.AvetonRoles;
using WebApi.DTOs.AvetonUser;
using WebApi.DTOs.Chat;
using WebApi.DTOs.ChatMember;
using WebApi.DTOs.ChatMessage;
using WebApi.DTOs.Client;
using WebApi.DTOs.Division;
using WebApi.DTOs.DivisionContractor;
using WebApi.DTOs.Employees;
using WebApi.DTOs.Job;
using WebApi.DTOs.Organization;
using WebApi.DTOs.Person;
using WebApi.DTOs.Position;
using WebApi.DTOs.Project;
using WebApi.DTOs.ProjectStage;
using WebApi.DTOs.StageManager;
using WebApi.DTOs.StageReport;
using WebApi.DTOs.StageReportAttachedFile;

namespace Mappings
{
    public class AutoMapperMappingProfile : Profile
    {
        public AutoMapperMappingProfile()
        {
            EntitiesToModels();
            ModelsToEntities();
            ModelsToDTOs();
            DTOsToModels();
            GlobalSettings();
        }

        private void EntitiesToModels()
        {
            CreateMap<EntityBase, ModelBase>()                
                .ForMember(x => x.EntityOwner, opt => opt.Ignore())
                .IncludeAllDerived();
            CreateMap<AvetonUser, GetAvetonUserModel>();
            CreateMap<Employee, GetEmployeeModel>()
                .ForMember(
                    destModel => destModel.Roles, 
                    opt => opt.MapFrom(source => source.Credentials.Roles)
                    );
            CreateMap<AvetonRole, GetAvetonRoleModel>();
            CreateMap<AvetonRoleAccess, GetAvetonRoleAccessModel>();
            CreateMap<Division, GetDivisionModel>();
            CreateMap<Division, GetDivisionWithChildsModel>();
            CreateMap<Position, GetPositionModel>();
            CreateMap<Job, GetJobModel>()
                .ForMember(
                    destModel => destModel.PositionName,
                    opt => opt.MapFrom(source => source.Position.Name)
                    )
                .ForMember(
                    destModel => destModel.DivisionName,
                    opt => opt.MapFrom(source => source.Division.Name)
                    );
            CreateMap<Organization, GetOrganizationModel>();
            CreateMap<Person, GetPersonModel>();
            CreateMap<Client, GetClientModel>();            
            CreateMap<Employee, GetEmployeeShortModel>();
            CreateMap<ProjectStage, GetProjectStageModel>()
                .ForMember(
                    destModel => destModel.Contractors,
                    opt => opt.MapFrom(source => source.NavigationDivisionContractors)
                    );
            CreateMap<Project, GetProjectModel>()
                .ForMember(
                    destModel => destModel.Contractors,
                    opt => opt.MapFrom(source => source.Stages.SelectMany(e => e.Contractors))
                    );
            CreateMap<StageManager, GetStageManagerModel>();
            CreateMap<StageReport, GetStageReportModel>();
            CreateMap<DivisionContractor, GetDivisionContractorModel>();
            CreateMap<StageReportAttachedFile, GetStageReportAttachedFileModel>();
            CreateMap<ChatMessage, GetChatMessageModel>();
            CreateMap<ChatMember, GetChatMemberModel>();
            CreateMap<Chat, GetChatModel>()
                .ForMember(
                    destModel => destModel.LastMessage,
                    opt => opt.MapFrom(source => source.LastMessageProjectable)
                    )
                .ForMember(
                    destModel => destModel.Messages,
                    opt => opt.Ignore()
                    );
            CreateMap<Organization, GetChatOrganizationModel>();
            CreateMap<Person, GetChatPersonModel>();
            CreateMap<Employee, GetChatEmployeeModel>();
            CreateMap<ChatMessageViewedInfo, GetChatMessageViewedInfoModel>();
            CreateMap<ChatMessageAttachedFile, GetChatMessageAttachedFileModel>();
        }

        private void ModelsToEntities()
        {
            CreateMap<CreateEmployeeModel, Employee>();
            CreateMap<UpdateEmployeeModel, Employee>();
            CreateMap<CreateAvetonRoleModel, AvetonRole>();
            CreateMap<UpdateAvetonRoleModel, AvetonRole>()
                .ForMember(x => x.Accesses, opt => opt.Ignore());
            CreateMap<UpdateAvetonRoleAccessModel, AvetonRoleAccess>();
            CreateMap<UpdateDivisionModel, Division>();
            CreateMap<CreateDivisionModel, Division>();
            CreateMap<UpdatePositionModel, Position>();
            CreateMap<CreatePositionModel, Position>();
            CreateMap<UpdateJobModel, Job>();
            CreateMap<CreateJobModel, Job>();
            CreateMap<UpdatePersonModel, Person>();
            CreateMap<CreatePersonModel, Person>();
            CreateMap<UpdateOrganizationModel, Organization>();
            CreateMap<CreateOrganizationModel, Organization>();
            CreateMap<UpdateClientModel, Client>();
            CreateMap<CreateClientModel, Client>();
            CreateMap<UpdateProjectStageModel, ProjectStage>();
            CreateMap<CreateProjectStageModel, ProjectStage>();
            CreateMap<UpdateProjectModel, Project>();
            CreateMap<CreateProjectModel, Project>();
            CreateMap<CreateStageManagerModel, StageManager>();
            CreateMap<UpdateStageManagerModel, StageManager>();
            CreateMap<CreateStageReportModel, StageReport>()
                .ForMember(x => x.AttachedFiles, opt => opt.Ignore());
            CreateMap<UpdateStageReportModel, StageReport>();
            CreateMap<CreateDivisionContractorModel, DivisionContractor>();
            CreateMap<UpdateDivisionContractorModel, DivisionContractor>();            
            CreateMap<UpdateChatModel, Chat>()
                .ForMember(x => x.ChatMembers, opt => opt.Ignore());
            CreateMap<CreateChatMemberForNewChatModel, ChatMember>();
            CreateMap<CreateChatFirstMessageModel, ChatMessage>();
            CreateMap<CreateChatMessageModel, ChatMessage>();
            CreateMap<UpdateChatMessageModel, ChatMessage>();
            CreateMap<CreateChatModel, Chat>();
            CreateMap<CreateChatMemberModel, ChatMember>();
            CreateMap<UpdateChatMemberModel, ChatMember>();
        }

        private void ModelsToDTOs()
        {
        }

        private void DTOsToModels()
        {
            CreateMap<ModelBase, DTOBase>()
                .ForMember(x => x.EntityOwner, opt => opt.Ignore())
                .IncludeAllDerived();
            CreateMap<LoginUserRequest, LoginUserModel>();
            CreateMap<CreateEmployeeRequest, CreateEmployeeModel>();
            CreateMap<CreateAvetonRoleRequest, CreateAvetonRoleModel>();
            CreateMap<UpdateAvetonRoleRequest, UpdateAvetonRoleModel>();
            CreateMap<UpdateAvetonRoleAccessRequest, UpdateAvetonRoleAccessModel>();
            CreateMap<CreateDivisionRequest, CreateDivisionModel>();
            CreateMap<UpdateDivisionRequest, UpdateDivisionModel>();
            CreateMap<CreatePositionRequest, CreatePositionModel>();
            CreateMap<UpdatePositionRequest, UpdatePositionModel>();
            CreateMap<UpdateEmployeeRequest, UpdateEmployeeModel>();
            CreateMap<UpdateAvetonUserRequest, UpdateAvetonUserModel>();
            CreateMap<CreateAvetonUserRequest, CreateAvetonUserModel>();
            CreateMap<CreateJobRequest, CreateJobModel>();
            CreateMap<UpdateJobRequest, UpdateJobModel>();
            CreateMap<CreatePersonRequest, CreatePersonModel>();
            CreateMap<UpdatePersonRequest, UpdatePersonModel>();
            CreateMap<CreateOrganizationRequest, CreateOrganizationModel>();
            CreateMap<UpdateOrganizationRequest, UpdateOrganizationModel>();
            CreateMap<CreateClientRequest, CreateClientModel>();
            CreateMap<UpdateClientRequest, UpdateClientModel>();
            CreateMap<CreateProjectRequest, CreateProjectModel>();
            CreateMap<UpdateProjectRequest, UpdateProjectModel>();
            CreateMap<CreateProjectStageRequest, CreateProjectStageModel>();
            CreateMap<UpdateProjectStageRequest, UpdateProjectStageModel>();
            CreateMap<CreateDivisionContractorRequest, CreateDivisionContractorModel>();
            CreateMap<CreateStageManagerRequest, CreateStageManagerModel>();
            CreateMap<CreateStageReportRequest, CreateStageReportModel>();
            CreateMap<UpdateStageReportRequest, UpdateStageReportModel>();
            CreateMap<CreateStageReportAttachedFileRequest, CreateStageReportAttachedFileModel>();
            CreateMap<LoginClientRequest, LoginClientModel>();
            CreateMap<CreateChatMemberForNewChatRequest, CreateChatMemberForNewChatModel>();
            CreateMap<CreateChatFirstMessageRequest, CreateChatFirstMessageModel>()
                .ForMember(
                    destModel => destModel.AttachFiles,
                    opt => opt.MapFrom(source => source.AttachedFiles!.Select(e => e.FileContent))
                    );
            CreateMap<CreateChatRequest, CreateChatModel>();
            CreateMap<CreateChatMessageRequest, CreateChatMessageModel>()
                .ForMember(
                    destModel => destModel.AttachFiles,
                    opt => opt.MapFrom(source => source.AttachedFiles!.Select(e => e.FileContent))
                    );
            CreateMap<UpdateChatRequest, UpdateChatModel>();
        }

        private void GlobalSettings()
        {

        }
    }
}
