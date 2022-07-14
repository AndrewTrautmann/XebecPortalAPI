using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.Shared;
using XebecAPI.Shared.Security;

namespace XebecAPI.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {

        /*Authentication*/
        IGenericRepository<AppUser> AppUsers { get; }
        IGenericRepository<KeyAssigner> KeysAssigned { get; }
        /*Authentication*/

        //Model Repositories
        IGenericRepository<AdditionalInformation> AdditionalInformation { get; }
        IGenericRepository<Application> Applications { get; }
        IGenericRepository<ApplicationPhase> Phases { get; }
        IGenericRepository<ApplicationPhaseHelper> ApplicationPhaseHelpers { get; }
        IGenericRepository<Document> Documents { get; }
        IGenericRepository<Education> Education { get; }
        IGenericRepository<Job> Jobs { get; }
        IGenericRepository<JobType> JobTypes { get; }
        IGenericRepository<JobTypeHelper> JobTypeHelpers { get; }
        IGenericRepository<LoginHelper> LoginHelpers { get; }
        IGenericRepository<PersonalInformation> PersonalInformation { get; }
        IGenericRepository<RegisterHelper> RegisterHelpers { get; }
        IGenericRepository<WorkHistory> WorkHistory { get; }

        IGenericRepository<JobPlatform> JobPlatforms { get; }
        IGenericRepository<JobPlatformHelper> JobPlatformHelpers { get; }

        IGenericRepository<ProfilePortfolioLink> ProfilePortfolioLinks { get; }

        IGenericRepository<QuestionnaireApplicantForm> QuestionnaireApplicantForms { get; }

        IGenericRepository<QuestionnaireHRForm> QuestionnaireHRForms { get; }

        IGenericRepository<JobApplicationPhase> JobApplicationPhases { get; }

        IGenericRepository<Question> Questions { get; }

        IGenericRepository<AnswerType> AnswerTypes { get; }
        IGenericRepository<CollaboratorAssigned> CollaboratorsAssigned { get; }

        IGenericRepository<CandidateRecommender> CandidatesRecommender { get; }

        IGenericRepository<UnsuccessfulReason> UnsuccessfulReasons { get; }
        IGenericRepository<RejectedCandidate> RejectedCandidates { get; }
        IGenericRepository<JobAlert> JobAlerts { get; }

        IGenericRepository<Reference> References { get; }

        IGenericRepository<Skill> Skills { get; }

        IGenericRepository<SkillsBank> SkillsBanks { get; }

        IGenericRepository<Department> Departments { get; }

        IGenericRepository<CollaboratorQuestion> CollaboratorQuestions { get; }

        IGenericRepository<Admin> Admins { get; }

        IGenericRepository<MatricMark> MatricMarks { get; }
        IGenericRepository<Company> Companies { get; }
        IGenericRepository<Location> Locations { get; }
        IGenericRepository<Policy> Policies { get; }
        IGenericRepository<ProfilePicture> ProfilePictures { get; }
        IGenericRepository<AdditionalDocument> AdditionalDocuments { get; }

        //Saving to the DB
        Task Save();

    }
}
