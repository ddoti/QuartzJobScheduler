using System;
using System.Web.Script.Serialization;
using log4net;
using Quartz;

namespace QuartzJobScheduler.Jobs
{
	/// <summary>
	/// This class is just a generic runner that gets spun up by Quartz. This is to put a layer
	/// between executed jobs and Quartz and to be able to add thread management.
	/// </summary>
	public class JobRunner : IJobRunner
	{
		private ILog _log;

		protected ILog Log
		{
			get
			{
				if(_log == null)
					_log = LogManager.GetLogger(typeof(JobRunner));
				return _log;
			}
		}

		public void Execute(IJobExecutionContext context)
		{
			var info = context.JobDetail.JobDataMap.GetString("JobInfo");
			var jobInfo = new JavaScriptSerializer().Deserialize<JobInfo>(info);
			Execute(jobInfo);
		}

		public void Execute(IJobInfo jobInfo)
		{
			Console.WriteLine("Executing Job - " + jobInfo.Name);
			
			var jobDomain = AppDomain.CreateDomain("JobDomain", null, new AppDomainSetup{ShadowCopyFiles = "true"});
			var newType = jobDomain.Load(jobInfo.AssemblyName).GetType(jobInfo.JobType);
			var newJob = new JavaScriptSerializer().Deserialize(jobInfo.DataString, newType);
			
			Execute(newJob as ICustomJob);
			
			AppDomain.Unload(jobDomain);
		}

		public JobStatus Execute(ICustomJob job)
		{
			JobStatus status;
			var start = DateTime.Now;

			if (job != null)
			{
				try
				{
					Log.Info("Starting job: " + job.JobName);
					status = job.Run();
					Log.Info("Job has finished successfully.");
				}
				catch (Exception ex)
				{
					Log.Error("Error occured during job: " + job.JobName, ex);
					status = JobStatus.Failure;
				}
			}
			else
			{
				Console.WriteLine("Unable to cast job to ICustomJob: " + job.JobName);
				Log.Error("Unable to cast job to ICustomJob: " + job.JobName);
				status = JobStatus.Failure;
			}

			var end = DateTime.Now - start;

			Console.WriteLine("Finished Executing Job ({0}) : {1} - {2}s", job.JobName, status, end.TotalSeconds);
			return status;
		}
	}
}
