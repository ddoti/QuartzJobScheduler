using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;

namespace QuartzJobScheduler.Jobs
{
	[DataContract]
	public class LogMessageJob : CustomJob
	{
		[DataMember]
		public string Message;

		public override JobStatus Run()
		{
			//Log.Info(Message);
			return JobStatus.Success;
		}
	}
}
