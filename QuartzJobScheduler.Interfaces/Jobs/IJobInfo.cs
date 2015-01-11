using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace QuartzJobScheduler.Jobs
{
	public interface IJobInfo
	{
		string JobType { get; set; }
		string DataString { get; set; }
		string Name { get; set; }
		string AssemblyName { get; set; }
	}
}
