using System;
using log4net;

namespace QuartzJobScheduler.Jobs
{
	public abstract class CustomJob : ICustomJob
	{
		private ILog _log;
		public abstract JobStatus Run();

		public virtual string JobName { get { return GetType().Name; } }
		public virtual string AssemblyName { get { return GetType().Assembly.FullName; } }
		public virtual string TypeName { get { return GetType().FullName; } }

		protected ILog Log
		{
			get
			{
				if (_log == null)
					_log = LogManager.GetLogger(GetType());
				return _log;
			}
		}
	}
}
