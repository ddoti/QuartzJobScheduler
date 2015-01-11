using System;
using System.Linq;
using System.ServiceModel;
using System.Web.Script.Serialization;
using QuartzJobScheduler.Jobs;

namespace QuartzJobScheduler
{
	public class JobManager : IJobManager
	{
		private JobManagementClient _client;
		private static IJobManager _instance;
		public string Endpoint { get; private set; }

		public JobManager() : this("http://localhost:9091/WcfService/QuartzScheduler")
		{
			
		}

		public JobManager(string clientEndpoint)
		{
			Endpoint = clientEndpoint;
		}

		public static IJobManager Instance
		{
			get
			{
				if(_instance == null)
					_instance = new JobManager();
				return _instance;
			}
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
			if (HasOpenClient())
			{
				JobInfo info = GetJobInfo(job);
				Client.Proxy.QueueJob(info);
				return JobStatus.Queued;
			}
			return RunNow(job);
		}

		public JobStatus RunNow(ICustomJob job)
		{
			var jobRunner = new JobRunner();
			return jobRunner.Execute(job);
		}

		public JobStatus ScheduleMinuteJob(ICustomJob job, int minuteDelay)
		{
			return SceduleJob(job, (x) => Client.Proxy.ScheduleMinuteJob(x, minuteDelay));
		}

		#region Private Methods

		private JobStatus SceduleJob(ICustomJob job, Action<JobInfo> action)
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
			var openStates = new[] { CommunicationState.Opened, CommunicationState.Opening };
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
