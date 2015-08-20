using System;

namespace QuartzJobScheduler.Jobs
{
	public abstract class CustomJob : ICustomJob
	{
		public abstract JobStatus Run();
		public virtual string JobName { get { return GetType().Name; } }
		public virtual string AssemblyName { get { return GetType().Assembly.FullName; } }
		public virtual string TypeName { get { return GetType().FullName; } }
	}
}
