using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.Shared;

namespace XebecAPI.IRepositories.CustomIRepositories
{
    public interface IMyJobsCustomRepo
    {
        Task<List<Application>> GetAllApplicationDetails(int AppUserId);
    }
}
