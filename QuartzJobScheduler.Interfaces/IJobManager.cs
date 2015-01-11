using QuartzJobScheduler.Jobs;

namespace QuartzJobScheduler
{
	public interface IJobManager
	{
		JobStatus QueueJob(ICustomJob job);
		JobStatus RunNow(ICustomJob job);

		//JobStatus ScheduleDailyJob(ICustomJob job);
		//JobStatus ScheduleHourlyJob(ICustomJob job, int hourDelay);
		JobStatus ScheduleMinuteJob(ICustomJob job, int minuteDelay);
	}
}
