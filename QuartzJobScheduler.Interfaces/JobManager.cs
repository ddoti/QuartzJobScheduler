using System;
using System.Linq;
using System.ServiceModel;
using System.Web.Script.Serialization;
using QuartzJobScheduler.Jobs;
using QuartzJobScheduler.Services;

namespace QuartzJobScheduler
{
	public class JobManager : IJobManager
	{
		private JobManagementClient _client;
		private IJobRunner _runner;
		public string Endpoint { get; private set; }

		public JobManager() : this("http://localhost:9091/WcfService/QuartzScheduler")
		{
			
		}

		public JobManager(string clientEndpoint) : this(clientEndpoint, new JobRunner())
		{
		}

		protected JobManager(string clientEndpoint, IJobRunner runner)
		{
			Endpoint = clientEndpoint;
			_runner = runner;
		}

		protected JobManagementClient Client
		{
			get
			{
				if(_client == null)
					_client = new JobManagementClient(Endpoint);
				return _client;
			}
		}

		public JobStatus QueueJob(ICustomJob job)
		{
			return ScheduleJob(job, Client.Proxy.QueueJob);
		}

		public JobStatus QueueJobWithDelay(ICustomJob job, TimeSpan delay)
		{
			return ScheduleJob(job, (x) => Client.Proxy.QueueJobWithDelay(x, delay));
		}

		public JobStatus ScheduleJobMinuteInterval(ICustomJob job, int minuteInterval, int limit = -1)
		{
			return ScheduleJob(job, (x) => Client.Proxy.ScheduleJobWithMinuteInterval(x, minuteInterval, limit));
		}

		public JobStatus ScheduleJobHourInterval(ICustomJob job, int hourInterval, int limit = -1)
		{
			return ScheduleJob(job, (x) => Client.Proxy.ScheduleJobWithHourlyInterval(x, hourInterval, limit));
		}

		public JobStatus ScheduleDailyJob(ICustomJob job, int limit = -1)
		{
			return ScheduleJob(job, (x) => Client.Proxy.ScheduleDailyJob(x, limit));
		}

		public JobStatus RunNow(ICustomJob job)
		{
			return _runner.Execute(job);
		}

		#region Private Methods

		private JobStatus ScheduleJob(ICustomJob job, Action<JobInfo> action)
		{
			if (HasOpenClient())
			{
				var info = GetJobInfo(job);
				action(info);
				return JobStatus.Queued;
			}

			return RunNow(job);
		}

		private JobInfo GetJobInfo(ICustomJob job)
		{
			return new JobInfo
			{
				JobType = job.TypeName,
				AssemblyName = job.AssemblyName,
				DataString = new JavaScriptSerializer().Serialize(job),
				Name = job.JobName
			};
		}

		private bool HasOpenClient()
		{
			var openStates = new[] { CommunicationState.Opened, CommunicationState.Opening, CommunicationState.Created };
			if (openStates.Contains(Client.State))
				return true;

			try
			{
				if (Client.State == CommunicationState.Created)
				{
					Client.Open();
					return true;
				}
			}
			catch (Exception ex)
			{

			}

			return false;
		}

		#endregion
	}
}
