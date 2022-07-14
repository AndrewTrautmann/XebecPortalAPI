using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XebecAPI.Data;
using XebecAPI.IRepositories.CustomIRepositories;
using XebecAPI.Shared;

namespace XebecAPI.Repositories.CustomRepositories
{
    public class JobsCustomRepo : GenericRepository<Job>, IJobsCustomRepo
    {
        public JobsCustomRepo(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Job>> GetAllJobsFullDetails()
        {
            return await _context.Jobs.Include(t => t.JobTypes).ThenInclude(x => x.JobType).Include(p => p.JobPlatforms).Include(z => z.Department).Include(q => q.Company).Include(x => x.Location).Include(x => x.Policy).AsNoTracking().ToListAsync();
        }

        public async Task<Job> GetJobTDetails(int JobId)
        {
            return await _context.Jobs.Where(j => j.Id == JobId).Include(t => t.JobTypes).ThenInclude(x => x.JobType).Include(p => p.JobPlatforms).Include(x => x.Department).Include(q => q.Company).Include(x => x.Location).Include(x => x.Policy).AsNoTracking().FirstAsync();
        }
    }
}
