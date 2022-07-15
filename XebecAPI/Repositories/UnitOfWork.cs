using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.Data;
using XebecAPI.IRepositories;
using XebecAPI.Shared;
using XebecAPI.Shared.Security;

namespace XebecAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        /*Authentication*/
        private IGenericRepository<AppUser> _appusers;

        private IGenericRepository<KeyAssigner> _keysassigned;
        /*Authentication*/

        private readonly ApplicationDbContext _context;

        private IGenericRepository<Application> _applications;
        private IGenericRepository<AdditionalInformation> _additionalInfo;
        private IGenericRepository<ApplicationPhase> _appPhases;
        private IGenericRepository<ApplicationPhaseHelper> _appPhaseHelper;
        private IGenericRepository<Document> _document;
      
        private IGenericRepository<Education> _education;
  
        private IGenericRepository<Job> _jobs;
        private IGenericRepository<JobType> _jobtypes;
        private IGenericRepository<JobTypeHelper> _jobTypeHelpers;
        private IGenericRepository<LoginHelper> _LoginHelpers;
        private IGenericRepository<PersonalInformation> _personalInfo;
        private IGenericRepository<RegisterHelper> _RegisterHelpers;
        
        private IGenericRepository<WorkHistory> _workHistories;

      
        private IGenericRepository<JobPlatform> _jobPlatforms;
        private IGenericRepository<JobPlatformHelper> _jobPlatformHelpers;

       
        private IGenericRepository<ProfilePortfolioLink> _profilePortfolioLinks;

        private IGenericRepository<QuestionnaireHRForm> _customQuestionsForHR;
        private IGenericRepository<QuestionnaireApplicantForm> _customQuestionsForApplicant;
        private IGenericRepository<JobApplicationPhase> _jobApplicationPhase;
        private IGenericRepository<Question> _question;
        private IGenericRepository<AnswerType> _answerType;
        private IGenericRepository<CollaboratorAssigned> _collaboratorsAssigned;
        private IGenericRepository<CandidateRecommender> _candidatesRecommender;
        private IGenericRepository<UnsuccessfulReason> _unsuccessfulReason;
        private IGenericRepository<RejectedCandidate> _rejectedCandidate;
        private IGenericRepository<JobAlert> _jobArlets;
        private IGenericRepository<Reference> _references;
        private IGenericRepository<Skill> _skills;
        private IGenericRepository<SkillsBank> _skillsBank;
        private IGenericRepository<Admin> _admins;

        private IGenericRepository<Department> _departments;
        private IGenericRepository<CollaboratorQuestion> _collaboratorQuestion;

        private IGenericRepository<MatricMark> _matricMarks;

        private IGenericRepository<Company> _companies;
        private IGenericRepository<Location> _locations;
        private IGenericRepository<Policy> _policies;
        private IGenericRepository<ProfilePicture> _profilePictures;
        private IGenericRepository<AdditionalDocument> _additionalDocuments;
        private IGenericRepository<CVDocument> _cvDocuments;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<KeyAssigner> KeysAssigned => _keysassigned ??= new GenericRepository<KeyAssigner>(_context);
        public IGenericRepository<AppUser> AppUsers => _appusers ??= new GenericRepository<AppUser>(_context);

        public IGenericRepository<AdditionalInformation> AdditionalInformation => _additionalInfo ??= new GenericRepository<AdditionalInformation>(_context);

        public IGenericRepository<Application> Applications => _applications ??= new GenericRepository<Application>(_context);

        public IGenericRepository<ApplicationPhase> Phases => _appPhases ??= new GenericRepository<ApplicationPhase>(_context);

        public IGenericRepository<ApplicationPhaseHelper> ApplicationPhaseHelpers => _appPhaseHelper ??= new GenericRepository<ApplicationPhaseHelper>(_context);

        public IGenericRepository<Document> Documents => _document ??= new GenericRepository<Document>(_context);

        public IGenericRepository<Education> Education => _education ??= new GenericRepository<Education>(_context);

        public IGenericRepository<Job> Jobs => _jobs ??= new GenericRepository<Job>(_context);

        public IGenericRepository<JobType> JobTypes => _jobtypes ??= new GenericRepository<JobType>(_context);

        public IGenericRepository<JobTypeHelper> JobTypeHelpers => _jobTypeHelpers ??= new GenericRepository<JobTypeHelper>(_context);

        public IGenericRepository<LoginHelper> LoginHelpers => _LoginHelpers ??= new GenericRepository<LoginHelper>(_context);

        public IGenericRepository<PersonalInformation> PersonalInformation => _personalInfo ??= new GenericRepository<PersonalInformation>(_context);

        public IGenericRepository<RegisterHelper> RegisterHelpers => _RegisterHelpers ??= new GenericRepository<RegisterHelper>(_context);

        public IGenericRepository<WorkHistory> WorkHistory => _workHistories ??= new GenericRepository<WorkHistory>(_context);

        public IGenericRepository<JobPlatform> JobPlatforms => _jobPlatforms ??= new GenericRepository<JobPlatform>(_context);

        public IGenericRepository<JobPlatformHelper> JobPlatformHelpers => _jobPlatformHelpers ??= new GenericRepository<JobPlatformHelper>(_context);

        public IGenericRepository<ProfilePortfolioLink> ProfilePortfolioLinks => _profilePortfolioLinks??= new GenericRepository<ProfilePortfolioLink>(_context);

        public IGenericRepository<QuestionnaireApplicantForm> QuestionnaireApplicantForms => _customQuestionsForApplicant ??= new GenericRepository<QuestionnaireApplicantForm>(_context);

        public IGenericRepository<QuestionnaireHRForm> QuestionnaireHRForms => _customQuestionsForHR ??= new GenericRepository<QuestionnaireHRForm>(_context);

        public IGenericRepository<JobApplicationPhase> JobApplicationPhases => _jobApplicationPhase ??= new GenericRepository<JobApplicationPhase>(_context);

        public IGenericRepository<Question> Questions => _question ??= new GenericRepository<Question>(_context);

        public IGenericRepository<AnswerType> AnswerTypes => _answerType ??= new GenericRepository<AnswerType>(_context);
        public IGenericRepository<CollaboratorAssigned> CollaboratorsAssigned => _collaboratorsAssigned ??= new GenericRepository<CollaboratorAssigned>(_context);

        public IGenericRepository<CandidateRecommender> CandidatesRecommender => _candidatesRecommender ??= new GenericRepository<CandidateRecommender>(_context);

        public IGenericRepository<UnsuccessfulReason> UnsuccessfulReasons => _unsuccessfulReason ??= new GenericRepository<UnsuccessfulReason>(_context);

        //RejectedCandidate
        public IGenericRepository<RejectedCandidate> RejectedCandidates => _rejectedCandidate ??= new GenericRepository<RejectedCandidate>(_context);
        public IGenericRepository<JobAlert> JobAlerts => _jobArlets ??= new GenericRepository<JobAlert>(_context);

        public IGenericRepository<Reference> References => _references ??= new GenericRepository<Reference>(_context);

        public IGenericRepository<Skill> Skills => _skills ??= new GenericRepository<Skill>(_context);

        public IGenericRepository<SkillsBank> SkillsBanks => _skillsBank ??= new GenericRepository<SkillsBank>(_context);

        public IGenericRepository<Admin> Admins => _admins ??= new GenericRepository<Admin>(_context);

        public IGenericRepository<Department> Departments => _departments ??= new GenericRepository<Department>(_context);

        public IGenericRepository<CollaboratorQuestion> CollaboratorQuestions => _collaboratorQuestion ??= new GenericRepository<CollaboratorQuestion>(_context);

        public IGenericRepository<MatricMark> MatricMarks => _matricMarks ??= new GenericRepository<MatricMark>(_context);
        public IGenericRepository<Company> Companies  => _companies ??= new GenericRepository<Company>(_context);
        public IGenericRepository<Location> Locations  => _locations ??= new GenericRepository<Location>(_context);
        public IGenericRepository<Policy> Policies  => _policies ??= new GenericRepository<Policy>(_context);
        public IGenericRepository<ProfilePicture> ProfilePictures  => _profilePictures ??= new GenericRepository<ProfilePicture>(_context);
        public IGenericRepository<AdditionalDocument> AdditionalDocuments=> _additionalDocuments ??= new GenericRepository<AdditionalDocument>(_context);
        public IGenericRepository<CVDocument> CVDocuments => _cvDocuments ??= new GenericRepository<CVDocument>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

    }
}
