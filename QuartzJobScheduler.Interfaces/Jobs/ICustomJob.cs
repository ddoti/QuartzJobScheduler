using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuartzJobScheduler.Jobs
{
	public interface ICustomJob
	{
		JobStatus Run();
		string JobName { get; }
		string AssemblyName { get; }
		string TypeName { get; }
	}
}
