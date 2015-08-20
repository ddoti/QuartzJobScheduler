using System;
using log4net;
using Quartz;
using QuartzJobScheduler.Helpers;
using QuartzJobScheduler.Jobs;
using QuartzJobScheduler.Services;

namespace QuartzJobScheduler
{
	public class JobManagementService : IJobManagementService
	{
		private ILog _log;

		public JobManagementService() : this(LogManager.GetLogger(typeof(JobManagementService)))
		{
		}

		public JobManagementService(ILog log)
		{
			_log = log;
		}

		public void HelloWorld(string message)
		{
			_log.Info("Message: " + message);
		}

		public void ScheduleCronJob(JobInfo job, string cronExpression)
		{
			IJobDetail jobDetail = JobBuilder.Create(job.GetType()).UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = TriggerBuilder.Create().StartNow().WithCronSchedule(cronExpression).Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		public void ScheduleCustomJob(JobInfo job)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = TriggerBuilder.Create().StartNow().Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		public void ScheduleDailyJob(JobInfo job, int limit)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = SimpleRepeatTriggerBuilder(TriggerBuilder.Create().StartNow(), (x) => x.WithIntervalInHours(24), limit);
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger); throw new NotImplementedException();
		}

		public void ScheduleJobWithHourlyInterval(JobInfo job, int hourDelay, int limit)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = SimpleRepeatTriggerBuilder(TriggerBuilder.Create().StartNow(), (x) => x.WithIntervalInHours(hourDelay), limit);
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		public void ScheduleJobWithMinuteInterval(JobInfo job, int minuteDelay, int limit)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = SimpleRepeatTriggerBuilder(TriggerBuilder.Create().StartNow(), (x) => x.WithIntervalInMinutes(minuteDelay), limit);
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		public void QueueJob(JobInfo job)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = TriggerBuilder.Create().StartNow().Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		public void QueueJobWithDelay(JobInfo job, TimeSpan delay)
		{
			var startTime = DateTime.Now.Add(delay);
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = TriggerBuilder.Create().StartAt(startTime).Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}


		#region private methods


		private ITrigger SimpleRepeatTriggerBuilder(TriggerBuilder builder, Func<SimpleScheduleBuilder, SimpleScheduleBuilder> func, int limit)
		{
			if (limit <= 0)
				return builder.WithSimpleSchedule(x => func(x).RepeatForever()).Build();
			return builder.WithSimpleSchedule(x => func(x).WithRepeatCount(limit)).Build();
		}

		#endregion
	}
}
