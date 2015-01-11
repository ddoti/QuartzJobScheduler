using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using QuartzJobScheduler.Helpers;
using QuartzJobScheduler.Services;

namespace QuartzJobScheduler
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
