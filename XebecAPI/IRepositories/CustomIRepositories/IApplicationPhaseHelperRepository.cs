using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.DTOs.ViewModels;
using XebecAPI.Shared;
using XebecAPI.Shared.Security;

namespace XebecAPI.IRepositories.CustomIRepositories
{
    public interface IApplicationPhaseHelperRepository
    {
        Task<List<ApplicationPhaseHelper>> GetApplicationPhaseInfo(int AppUserId);

        Task<List<ApplicationPhaseHelper>> GetApplicationPhaseInfoDetailed(int AppUserId, int jobId);

        Task<List<ApplicantViewModel>> GetApplicants(int jobId);

        Task<List<myJobsViewModel>> GetApplicationPhaseInfoForUser(int AppUserId, int PhaseId);

        Task<List<ApplicantViewModel>> GetallApplicants();
        Task<List<ApplicantPortalView>> GetApplicantsForJob(int JobId);
    }
}
