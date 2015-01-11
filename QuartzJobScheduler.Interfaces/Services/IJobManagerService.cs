using QuartzJobScheduler.Jobs;

namespace QuartzJobScheduler.Services
{
	public interface IJobManagerService
	{
		JobStatus QueueJob(ICustomJob job);
		JobStatus RunNow(ICustomJob job);
		JobStatus ScheduleCustomJob(ICustomJob job);
	}
}
