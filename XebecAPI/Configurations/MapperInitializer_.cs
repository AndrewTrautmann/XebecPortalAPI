using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using XebecAPI.DTOs;
using XebecAPI.Shared;
using XebecAPI.Shared.Security;

namespace XebecAPI.Configurations
{
    public class MapperInitializer_ : Profile
    {
        public MapperInitializer_()
        {
            CreateMap<AppUser, AppUserDTO>().ReverseMap();
            CreateMap<Job, JobDTO>().ReverseMap();
            CreateMap<AdditionalInformation, AdditionalInformationDTO>().ReverseMap();
            CreateMap<Application, ApplicationDTO>().ReverseMap();
            CreateMap<ApplicationPhase, ApplicationPhaseDTO>().ReverseMap();
            CreateMap<ApplicationPhaseHelper, ApplicationPhaseHelperDTO>().ReverseMap();
            CreateMap<Document, DocumentsDTO>().ReverseMap();
            CreateMap<Education, EducationDTO>().ReverseMap();
            CreateMap<JobPlatform, JobPlatformDTO>().ReverseMap();
            CreateMap<JobPlatformHelper, JobPlatformHelperDTO>().ReverseMap();
            CreateMap<JobType, JobTypeDTO>().ReverseMap();
            CreateMap<JobTypeHelper, JobTypeHelperDTO>().ReverseMap();
            CreateMap<LoginHelper, LoginHelperDTO>().ReverseMap();
            CreateMap<PersonalInformation, PersonalInformationDTO>().ReverseMap();
            CreateMap<ProfilePortfolioLink, ProfilePortfolioLinkDTO>().ReverseMap();
            CreateMap<WorkHistory, WorkHistoryDTO>().ReverseMap();
            CreateMap<JobApplicationPhase, JobApplicationPhaseDTO>().ReverseMap();
            CreateMap<QuestionnaireApplicantForm, QuestionnaireApplicantFormDTO>().ReverseMap();
            CreateMap<QuestionnaireHRForm, QuestionnaireHRFormDTO>().ReverseMap();
            CreateMap<AnswerType, AnswerTypeDTO>().ReverseMap();
            CreateMap<CollaboratorAssigned, CollaboratorsAssignedDTO>().ReverseMap();
            CreateMap<CandidateRecommender, CandidateRecommenderDTO>().ReverseMap();
            CreateMap<UnsuccessfulReason, UnsuccessfulReasonDTO>().ReverseMap();
            CreateMap<RejectedCandidate, RejectedCandidateDTO>().ReverseMap();
            CreateMap<JobAlert, JobArletsDTO>().ReverseMap();
            CreateMap<Reference, ReferenceDTO>().ReverseMap();
            CreateMap<Skill, SkillDTO>().ReverseMap();
            CreateMap<SkillsBank, SkillsBankDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<CollaboratorQuestion, CollaboratorQuestionDTO>().ReverseMap();
            CreateMap<MatricMark, MatricMarkDTO>().ReverseMap();
            CreateMap<Company, CompanyDTO>().ReverseMap();
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Policy, PolicyDTO>().ReverseMap();
            CreateMap<ProfilePicture, ProfilePictureDTO>().ReverseMap();
        }

    }
}
