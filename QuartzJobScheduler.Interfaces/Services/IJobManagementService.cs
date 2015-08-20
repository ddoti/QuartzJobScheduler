using System;
using System.ServiceModel;
using QuartzJobScheduler.Jobs;

namespace QuartzJobScheduler.Services
{
	[ServiceContract]
	public interface IJobManagementService
	{
		[OperationContract]
		void HelloWorld(string message);

		[OperationContract]
		void ScheduleCronJob(JobInfo job, string cronExpression);

		[OperationContract]
		void ScheduleCustomJob(JobInfo job);

		[OperationContract]
		void ScheduleDailyJob(JobInfo job, int limit);

		[OperationContract]
		void ScheduleJobWithHourlyInterval(JobInfo job, int hourDelay, int limit);

		[OperationContract]
		void ScheduleJobWithMinuteInterval(JobInfo job, int minuteDelay, int limit);

		[OperationContract]
		void QueueJob(JobInfo job);

		[OperationContract]
		void QueueJobWithDelay(JobInfo job, TimeSpan delay);
	}
}
