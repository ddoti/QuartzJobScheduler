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

		public void ScheduleDailyJob(JobInfo job)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever()).Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		public void ScheduleHourlyJob(JobInfo job, int hourDelay)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInHours(hourDelay).RepeatForever()).Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		public void ScheduleMinuteJob(JobInfo job, int minuteDelay)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInMinutes(minuteDelay).RepeatForever()).Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		public void QueueJob(JobInfo job)
		{
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = TriggerBuilder.Create().StartNow().Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}
	}
}
