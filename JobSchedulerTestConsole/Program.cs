using System;
using QuartzJobScheduler;
using QuartzJobScheduler.Jobs;

namespace JobSchedulerTestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var job = new LogMessageJob {Message = "This is my first message"};

			//JobManager.RunNow(job);
			//JobManager.QueueJob(job);
			var manager = new JobManager();

			var dumb = new DumbJob();
			manager.RunNow(dumb);
			manager.QueueJob(dumb);

			Console.WriteLine("Hit ENTER to continue.");
			//Console.ReadLine();
		}
	}

	public class DumbJob : CustomJob
	{
		public override JobStatus Run()
		{
			Console.WriteLine("I am a dumb job!");
			return JobStatus.Success;
		}
	}
}
