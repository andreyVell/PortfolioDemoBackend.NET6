using DataCore.Entities;
using Services.Models.Job;

namespace Services.Interfaces
{
    public interface IJobService : ICrudService<Job, GetJobModel, CreateJobModel, UpdateJobModel>
    {
    }
}
