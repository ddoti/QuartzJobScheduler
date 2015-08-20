using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using QuartzJobScheduler.Services;

namespace QuartzJobScheduler.Services
{
	public class JobManagementClient : GenericWebClient<IJobManagementService>
	{
		public JobManagementClient(string endpointAddress) : this(new WSHttpBinding(), endpointAddress)
		{
			
		}

		public JobManagementClient(Binding binding, string endpointAddress) : base(binding, new EndpointAddress(endpointAddress))
		{
			
		}
	}
}
