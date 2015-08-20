using System;
using QuartzJobScheduler.Jobs;

namespace QuartzJobScheduler
{
	public interface IJobManager
	{
		JobStatus RunNow(ICustomJob job);

		//JobStatus ScheduleDailyJob(ICustomJob job);
		//JobStatus ScheduleHourlyJob(ICustomJob job, int hourDelay);

		JobStatus QueueJob(ICustomJob job);
		JobStatus QueueJobWithDelay(ICustomJob job, TimeSpan delay);

		JobStatus ScheduleJobMinuteInterval(ICustomJob job, int minuteInterval, int limit = -1);
		JobStatus ScheduleJobHourInterval(ICustomJob job, int hourInterval, int limit = -1);
		JobStatus ScheduleDailyJob(ICustomJob job, int limit = -1);
	}
}
