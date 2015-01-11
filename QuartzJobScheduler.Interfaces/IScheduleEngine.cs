using Quartz;

namespace QuartzJobScheduler
{
	public interface IScheduleEngine
	{
		IScheduler Scheduler { get; }
	}
}
