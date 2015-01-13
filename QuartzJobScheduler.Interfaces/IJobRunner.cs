using QuartzJobScheduler.Jobs;

namespace QuartzJobScheduler
{
	public interface IJobRunner
	{
		void Execute(IJobInfo jobInfo);
		JobStatus Execute(ICustomJob job);
	}
}
