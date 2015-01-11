using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace QuartzJobScheduler.Jobs
{
	[DataContract]
	public class CommandLineJob : CustomJob
	{
		[DataMember] 
		public string ExecutiblePath;
		[DataMember] 
		public string Arguments;
		[DataMember] 
		public bool WaitForExit = true;

		public override JobStatus Run()
		{
			if(String.IsNullOrEmpty(ExecutiblePath))
				throw new Exception("Executible path cannot be null or empty.");

			var exeProc = new ProcessStartInfo {WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true, FileName = ExecutiblePath};
			exeProc.Arguments = Arguments;

			using (var proc = Process.Start(exeProc))
			{
				if (WaitForExit)
				{
					proc.WaitForExit();
					if (proc.ExitCode == 0)
					{
						return JobStatus.Success;
					}
					return JobStatus.Failure;
				}
			}

			return JobStatus.Success;
		}
	}
}
