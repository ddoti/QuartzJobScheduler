using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;

namespace QuartzJobScheduler.Jobs
{
	[DataContract]
	[KnownType(typeof(IJobInfo))]
	public class JobInfo : IJobInfo
	{
		[DataMember]
		public string JobType { get; set; }
		[DataMember]
		public string DataString { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember] 
		public string AssemblyName { get; set; }

		public override string ToString()
		{
			return new JavaScriptSerializer().Serialize(this);
		}
	}
}
