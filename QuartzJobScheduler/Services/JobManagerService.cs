using System;
using System.Linq;
using System.ServiceModel;
using System.Web.Script.Serialization;
using QuartzJobScheduler.Jobs;

namespace QuartzJobScheduler.Services
{
	public class JobManagerService : IJobManagerService
	{
		private JobManagementClient _client;
		private string _endpoint;

		public JobManagerService(string endpoint)
		{
			_client = new JobManagementClient(endpoint);
		}

		protected JobManagementClient Client
		{
			get
			{
				if (_client == null)
					_client = new JobManagementClient(_endpoint);
				return _client;
			}
		}

		public JobStatus QueueJob(ICustomJob job)
		{
			return ScheduleQueuedJob(job, Client.Proxy.QueueJob);
		}

		public JobStatus RunNow(ICustomJob job)
		{
			var jobRunner = new QuartzJobRunner();
			return jobRunner.Execute(job);
		}

		public JobStatus ScheduleCustomJob(ICustomJob job)
		{
			return ScheduleQueuedJob(job, Client.Proxy.ScheduleCustomJob);
		}

		#region Private Methods

		private JobStatus ScheduleQueuedJob(ICustomJob job, Action<JobInfo> action)
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
