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
		void ScheduleDailyJob(JobInfo job);

		[OperationContract]
		void ScheduleHourlyJob(JobInfo job, int hourDelay);

		[OperationContract]
		void ScheduleMinuteJob(JobInfo job, int minuteDelay);

		[OperationContract]
		void QueueJob(JobInfo job);
	}
}
