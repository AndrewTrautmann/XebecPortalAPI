using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using XebecAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.DTOs.ViewModels;
using XebecAPI.Shared;
using XebecAPI.Shared.Security;
using XebecAPI.IRepositories.CustomIRepositories;

namespace XebecAPI.Repositories.CustomRepositories
{
    public class UsersCustomRepo : GenericRepository<AppUser>, IUsersCustomRepo
    {
        public UsersCustomRepo(ApplicationDbContext context) : base(context)
        {
        }

        #region Experiemental Queries
        public async Task<List<CandidateViewModel>> GetCandidateDetails(int JobId)
        {
            IQueryable<CandidateViewModel> queryPI = null;
            IQueryable<AppUser> queryUsers = from users in _context.AppUser
                                             join applications in _context.Applications.Where(a => a.JobId == JobId)
                                             on users.Id equals applications.AppUserId
                                             select users;



            IQueryable<WorkHistory> works = from users in queryUsers
                                            join work in _context.WorkHistories
                                            on users.Id equals work.AppUserId
                                            select work;

            //GetApplicants for sepecific job
            queryPI = from users in queryUsers
                      join personal in _context.PersonalInformations
                      on users.Id equals personal.AppUserId
                      select new CandidateViewModel() { WorkHistories = works.ToList(), PersonalInfo = personal };

            return await queryPI.AsNoTracking().ToListAsync();
        }
        #endregion
        #region Tshego's queries
        public async Task<List<PersonalInformation>> GetApplicantsDetailsByJobId(int JobId)
        {
            IQueryable<PersonalInformation> queryFinal;
            //var job = new SqlParameter("jobId", JobId);
            //IQueryable<PersonalInformation> queryFinal = _context.PersonalInformations.
            //    FromSqlRaw("SELECT * from PersonalInformations Where UserId IN (SELECT UserId FROM Applications where JobId = @jobId)", job);

            queryFinal = from users in _context.AppUser
                         join applications in _context.Applications.Where(a => a.JobId == JobId)
                             on users.Id equals applications.AppUserId
                         join info in _context.PersonalInformations
                             on users.Id equals info.AppUserId
                         select info;

            return await queryFinal.AsNoTracking().ToListAsync();
        }
        public async Task<List<AppUser>> GetApplicantIds(int JobId)
        {
            IQueryable<AppUser> queryFinal;


            queryFinal = from users in _context.AppUser
                         join applications in _context.Applications.Where(a => a.JobId == JobId)
                             on users.Id equals applications.AppUserId
                         select users;

            return await queryFinal.AsNoTracking().ToListAsync();
        }

        public async Task<List<PersonalInformation>> SearchApplicants(int JobId, string SearchQuery, string ethnicityFiler, string GenderFilter, string disabilityFilter)
        {

            IQueryable<PersonalInformation> queryPI = _context.PersonalInformations;
            IQueryable<WorkHistory> queryWH = _context.WorkHistories;
            IQueryable<Education> queryEd = _context.Educations;

            //GetApplicants for sepecific job
            IQueryable<AppUser> query = from users in _context.AppUser
                                        join applications in _context.Applications.Where(a => a.JobId == JobId)
                                            on users.Id equals applications.AppUserId
                                        select users;
            //Search name and surname
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = from user in query
                        join personalinfo in _context.PersonalInformations.
                        Where(p => p.FirstName.Contains(SearchQuery) || p.LastName.Contains(SearchQuery))
                            on user.Id equals personalinfo.AppUserId
                        select user;
            }
            //filter by ethnicity
            if (!string.IsNullOrEmpty(ethnicityFiler))
            {
                query = from user in query
                        join person in _context.AdditionalInformations.Where(e => e.Ethnicity.Contains(ethnicityFiler))
                            on user.Id equals person.AppUserId
                        select user;
                //Searches for job titles, compensation and locations based on what user entered
            }
            //filter by gender
            if (!string.IsNullOrEmpty(GenderFilter))
            {
                query = from user in query
                        join person in _context.AdditionalInformations.Where(e => e.Gender.Equals(GenderFilter))
                            on user.Id equals person.AppUserId
                        select user;
                //Searches for job titles, compensation and locations based on what user entered
            }
            //filter by disability
            if (!string.IsNullOrEmpty(disabilityFilter))
            {
                query = from user in query
                        join person in _context.AdditionalInformations.Where(e => e.Disability == disabilityFilter)
                            on user.Id equals person.AppUserId
                        select user;
                //Searches for job titles, compensation and locations based on what user entered
            }
            queryPI = from users in query
                      join applications in queryPI
                          on users.Id equals applications.AppUserId
                      select applications;
            //return personalInfo of people after going through all the queries
            return await queryPI.AsNoTracking().ToListAsync();
        }
        #endregion
        #region Iviwe's queries
        public async Task<List<PersonalInformation>> GetPersonalInfoByRole(string role)
        {
            IQueryable<PersonalInformation> query;

            query = from users in _context.AppUser.Where(r => r.Role == role)
                    join info in _context.PersonalInformations
                     on users.Id equals info.AppUserId
                    select info;

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<List<PersonalInformation>> GetPersonalInfoByAppUsers(List<AppUser> lstUsers)
        {
            IQueryable<PersonalInformation> query;
            IQueryable<AppUser> queryUsers = lstUsers.AsQueryable();
            query = from users in queryUsers
                    join info in _context.PersonalInformations
                     on users.Id equals info.AppUserId
                    select info;

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<List<PersonalInformation>> GetPersonalByAdditional(string disability, string gender, string ethnicity)
        {
            IQueryable<PersonalInformation> queryPI = _context.PersonalInformations;

            if (!string.IsNullOrEmpty(disability))
            {
                queryPI = from user in queryPI
                          join personalinfo in _context.AdditionalInformations.
                          Where(p => p.Ethnicity.Contains(ethnicity))
                              on user.AppUserId equals personalinfo.AppUserId
                          select user;
                //filters by disability in additionalInfo table
            }

            if (!string.IsNullOrEmpty(ethnicity))
            {
                queryPI = from user in queryPI
                          join addition in _context.AdditionalInformations.Where(e => e.Disability == disability)
                              on user.AppUserId equals addition.AppUserId
                          select user;
                //filters by ethnicity in additionalInfo table
            }
            //filter by gender
            if (!string.IsNullOrEmpty(gender))
            {
                queryPI = from user in queryPI
                          join person in _context.AdditionalInformations.Where(e => e.Gender.Equals(gender))
                              on user.AppUserId equals person.AppUserId
                          select user;
                //filters by gender in additionalInfo table
            }

            return await queryPI.AsNoTracking().ToListAsync();
        }
        #endregion

        #region Kian's Queries

        public async Task<List<ProfileViewModel>> GetProfile(int AppUserId)
        {
            IQueryable<ProfileViewModel> queryPI = null;

            IQueryable<AppUser> queryUsers = from users in _context.AppUser.Where(p => p.Id == AppUserId)

                                             select users;



            IQueryable<AdditionalInformation> additionalInformation = from users in queryUsers
                                                                      join additional in _context.AdditionalInformations
                                                                      on users.Id equals additional.AppUserId
                                                                      select additional;


            IQueryable<Document> document = from users in queryUsers
                                            join doc in _context.Documents
                                            on users.Id equals doc.AppUserId
                                            select doc;


            IQueryable<PersonalInformation> personalInfo = from users in queryUsers
                                                           join personal in _context.PersonalInformations
                                                           on users.Id equals personal.AppUserId
                                                           select personal;

            IQueryable<Education> education = from users in queryUsers
                                              join education_ in _context.Educations
                                              on users.Id equals education_.AppUserId
                                              select education_;

            IQueryable<ProfilePortfolioLink> links = from users in queryUsers
                                                     join link in _context.ProfilePortfolioLinks
                                                     on users.Id equals link.AppUserId
                                                     select link;

            IQueryable<WorkHistory> workHistory = from users in queryUsers
                                                  join work in _context.WorkHistories
                                                  on users.Id equals work.AppUserId
                                                  select work;




            queryPI = from users in queryUsers
                      select new ProfileViewModel() { AdditionalInformation = additionalInformation.First(), Document = document.ToList(), PersonalInformation = personalInfo.First(), Educations = education.ToList(), ProfilePortfolioLink = links.First(), WorkHistories = workHistory.ToList() };

            return await queryPI.AsNoTracking().ToListAsync();

        }

        public async Task<List<QuestionnaireHRForm>> GetQuestions(int JobId)
        {
            IQueryable<QuestionnaireHRForm> queryPI = null;

            if (JobId != 0)
            {

                queryPI = from applicants in _context.QuestionnaireApplicantForms
                          join questions in _context.QuestionnaireHRForms.Where(a => a.JobId == JobId)
                              on applicants.QuestionnaireHRFormId equals questions.Id
                          select questions;

            }

            return await queryPI.AsNoTracking().ToListAsync();

        }
        #endregion
        public async Task<List<PersonalInformation>> SearchUser(string role, string name)
        {
            IQueryable<PersonalInformation> query;

            query = from users in _context.AppUser.Where(r => r.Role == role)
                    join info in _context.PersonalInformations.Where(n => n.FirstName == name)
                     on users.Id equals info.AppUserId
                    select info;

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<List<JobApplicationPhase>> SearchPhasebyJob(int job)
        {
            IQueryable<JobApplicationPhase> query;

            query = _context.JobApplicationPhases.Where(j => j.JobId == job);

            return await query.Include(z => z.ApplicationPhase).AsNoTracking().ToListAsync();
        }
    }
}


