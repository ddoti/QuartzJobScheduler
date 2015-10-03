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
			TriggerJob(job, x => x.WithCronSchedule(cronExpression));
		}

		public void ScheduleCustomJob(JobInfo job)
		{
			TriggerJob(job);
		}

		public void ScheduleDailyJob(JobInfo job, int limit)
		{
			ScheduleJobWithHourlyInterval(job, 24, limit);
		}

		public void ScheduleJobWithHourlyInterval(JobInfo job, int hourDelay, int limit)
		{
			TriggerSimpleJob(job, (x) => x.WithIntervalInHours(hourDelay), limit);
		}

		public void ScheduleJobWithMinuteInterval(JobInfo job, int minuteDelay, int limit)
		{
			TriggerSimpleJob(job, (x) => x.WithIntervalInMinutes(minuteDelay), limit);
		}

		public void QueueJob(JobInfo job)
		{
			TriggerJob(job);
		}

		public void QueueJobWithDelay(JobInfo job, TimeSpan delay)
		{
			TriggerDelayedJob(job, delay);
		}

		#region private methods

		private void TriggerSimpleJob(JobInfo job, Func<SimpleScheduleBuilder, SimpleScheduleBuilder> func, int limit = 1)
		{
			if (limit <= 0)
			{
				TriggerJob(job, (x) => x.WithSimpleSchedule(simple => func(simple).RepeatForever()));
			}

			TriggerJob(job, x => x.WithSimpleSchedule(simple => func(simple).WithRepeatCount(limit)));
		}

		private void TriggerJob(JobInfo job, Func<TriggerBuilder, TriggerBuilder> triggerFunc = null)
		{
			if(triggerFunc == null)
			{
				triggerFunc = (x) => x;
			}

			Console.WriteLine("{0} - Queueing job: {1}", DateTime.Now.ToLongTimeString(), job.JobType);
			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = triggerFunc(TriggerBuilder.Create().StartNow()).Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		private void TriggerDelayedJob(JobInfo job, TimeSpan delay, Func<TriggerBuilder, TriggerBuilder> triggerFunc = null)
		{
			if(triggerFunc == null)
			{
				triggerFunc = (x) => x;
			}

			var startTime = DateTime.Now.Add(delay);

			Console.WriteLine("{0} - Queued Delayed Job: {1} (Expected Runtime: {2})", DateTime.Now.ToLongTimeString(), job.JobType, startTime.ToLongTimeString());

			IJobDetail jobDetail = JobBuilder.Create<QuartzJobRunner>().UsingJobData("JobInfo", job.ToString()).Build();
			ITrigger trigger = triggerFunc(TriggerBuilder.Create().StartAt(startTime)).Build();
			ScheduleEngine.Instance.Scheduler.ScheduleJob(jobDetail, trigger);
		}

		#endregion
	}
}
