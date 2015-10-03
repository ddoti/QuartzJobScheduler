using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using log4net;
using QuartzJobScheduler.Jobs;

namespace QuartzJobScheduler
{
	public class JobRunner : IJobRunner
	{
		private ILog _log;

		protected ILog Log
		{
			get
			{
				if (_log == null)
					_log = LogManager.GetLogger(typeof(JobRunner));
				return _log;
			}
		}

		public void Execute(IJobInfo jobInfo)
		{
			Console.WriteLine("{0} - Executing Job - {1}", DateTime.Now.ToLongTimeString(), jobInfo.Name);

			ICustomJob job = null;
			AppDomain jobDomain = null;

			try
			{
				jobDomain = AppDomain.CreateDomain("JobDomain", null, new AppDomainSetup { ShadowCopyFiles = "true" });
				var newType = jobDomain.Load(jobInfo.AssemblyName).GetType(jobInfo.JobType);
				var newJob = new JavaScriptSerializer().Deserialize(jobInfo.DataString, newType);
				job = newJob as ICustomJob;
			}
			catch (Exception ex)
			{
				Console.WriteLine("{0} - Error: Failed to job type from assembly. JobType: {1} Assembly: {2}", DateTime.Now.ToLongTimeString(), jobInfo.JobType, jobInfo.AssemblyName);
			}

			Execute(job);
			AppDomain.Unload(jobDomain);
		}

		public JobStatus Execute(ICustomJob job = null)
		{
			JobStatus status;
			var start = DateTime.Now;

			if (job != null)
			{
				try
				{
					//Log.Info("Starting job: " + job.JobName);
					status = job.Run();
					//Log.Info("Job has finished successfully.");
				}
				catch (Exception ex)
				{
					//Log.Error("Error occured during job: " + job.JobName, ex);
					status = JobStatus.Failure;
				}
			}
			else
			{
				Console.WriteLine("Unable to cast job to ICustomJob: " + job.JobName);
				//Log.Error("Unable to cast job to ICustomJob: " + job.JobName);
				status = JobStatus.Failure;
			}

			var end = DateTime.Now - start;

			Console.WriteLine("{3} - Finished Executing Job ({0}) : {1} - {2}s", job.JobName, status, end.TotalSeconds, DateTime.Now.ToLongTimeString());
			return status;
		}
	}
}
