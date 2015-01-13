using System;
using System.Web.Script.Serialization;
using Quartz;

namespace QuartzJobScheduler.Jobs
{
	/// <summary>
	/// This class is just a generic runner that gets spun up by Quartz. This is to put a layer
	/// between executed jobs and Quartz and to be able to add thread management.
	/// </summary>
	public class QuartzJobRunner : JobRunner, IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			var info = context.JobDetail.JobDataMap.GetString("JobInfo");
			var jobInfo = new JavaScriptSerializer().Deserialize<JobInfo>(info);
			Execute(jobInfo);
		}
	}
}
