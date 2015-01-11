using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;

namespace QuartzJobScheduler.Helpers
{
	public class ScheduleEngine : IScheduleEngine
	{
		private IScheduler _scheduler;
		private static ScheduleEngine _instance;

		public ScheduleEngine() : this(new StdSchedulerFactory())
		{
		}

		public ScheduleEngine(ISchedulerFactory factory)
		{
			_scheduler = factory.GetScheduler();
			_scheduler.Start();
		}

		public static IScheduleEngine Instance
		{
			get
			{
				if (_instance == null)
					_instance = new ScheduleEngine();
				return _instance;
			}
		}

		public IScheduler Scheduler
		{
			get { return _scheduler; }
		}

		public static void Dispose()
		{
			Instance.Scheduler.Shutdown(false);
			_instance = null;
		}
	}
}
