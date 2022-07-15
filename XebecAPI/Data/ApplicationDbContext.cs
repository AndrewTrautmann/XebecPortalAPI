using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XebecAPI.Configurations;
using XebecAPI.Shared;
using XebecAPI.Shared.Security;

namespace XebecAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        /*Authentication*/
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<KeyAssigner> KeysAssigned { get; set; }
        /*Authentication*/

        public DbSet<AdditionalInformation> AdditionalInformations { get; set;}
        public DbSet<Application> Applications { get; set;}
        public DbSet<ApplicationPhase> ApplicationPhases { get; set; }
        public DbSet<ApplicationPhaseHelper> ApplicationPhasesHelpers { get; set;}
        public DbSet<Document> Documents { get; set;}
        public DbSet<Education> Educations { get; set;}
        public DbSet<Job> Jobs { get; set;}
        public DbSet<JobType> JobTypes { get; set;}
        public DbSet<JobTypeHelper> JobTypeHelpers { get; set;}
        public DbSet<LoginHelper> LoginHelpers { get; set;}
        public DbSet<PersonalInformation> PersonalInformations { get; set;}
        public DbSet<RegisterHelper> RegisterHelpers { get; set;}
        public DbSet<WorkHistory> WorkHistories { get; set;}
        public DbSet<JobPlatform> JobPlatforms { get; set; }
        public DbSet<JobPlatformHelper> JobPlatformHelpers { get; set; }
        public DbSet<ProfilePortfolioLink> ProfilePortfolioLinks { get; set; }
        public DbSet<QuestionnaireHRForm> QuestionnaireHRForms { get; set; }
        public DbSet<QuestionnaireApplicantForm> QuestionnaireApplicantForms { get; set; }
        public DbSet<JobApplicationPhase> JobApplicationPhases { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<AnswerType> AnswerTypes { get; set; }
        public DbSet<CollaboratorAssigned> CollaboratorsAssigneds { get; set; }
        public DbSet<CandidateRecommender> CandidatesRecommender { get; set; }

        public DbSet<UnsuccessfulReason> UnsuccessfulReasons { get; set; }
        public DbSet<RejectedCandidate> RejectedCandidates { get; set; }
        public DbSet<JobAlert> JobAlerts { get; set; }

        public DbSet<Reference> References { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<SkillsBank> SkillsBanks { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<CollaboratorQuestion> CollaboratorQuestions { get; set; }

        public DbSet<MatricMark> MatricMarks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }
        public DbSet<AdditionalDocument> AdditionalDocuments { get; set; }
        public DbSet<CVDocument> CVDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new AnswerTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new JobPlatformConfiguration());
            //modelBuilder.ApplyConfiguration(new JobTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new AppPhaseConfiguration());
            //modelBuilder.ApplyConfiguration(new UnsuccessfulReasonConfiguration());
        }

    }
}