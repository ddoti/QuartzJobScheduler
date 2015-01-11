using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using log4net;
using QuartzJobScheduler;
using QuartzJobScheduler.Helpers;
using QuartzJobScheduler.Services;

namespace JobSchedulerSvc
{
	public class Program
	{
		private static ILog _log;
		static void Main(string[] args)
		{
			try
			{
				AppDomain.CurrentDomain.SetShadowCopyFiles();

				log4net.Config.XmlConfigurator.Configure();
				_log = LogManager.GetLogger(typeof (Program));

				var address = ConfigurationManager.AppSettings["baseUrl"];
				_log.Info("Starting Service at " + address);

				using (ServiceHost host = new ServiceHost(typeof (JobManagementService), new Uri(address)))
				{
					var contract = ContractDescription.GetContract(typeof (IJobManagementService), typeof (JobManagementService));
					var binding = new WSHttpBinding();
					var endpoint = new EndpointAddress(address);
					var serviceEndpoint = new ServiceEndpoint(contract, binding, endpoint);

					host.Description.Endpoints.Add(serviceEndpoint);

					// Enable metadata publishing.
					ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
					smb.HttpGetEnabled = true;
					smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
					host.Description.Behaviors.Add(smb);

					host.Open();

					Console.WriteLine("The service is ready at {0}", address);
					Console.WriteLine("Press <Enter> to stop the service.\n\n");
					Console.ReadLine();

					ScheduleEngine.Dispose();

					host.Close();
				}
			}
			catch (Exception ex)
			{
				if (_log != null)
				{
					_log.Error(ex.Message, ex);
				}
				throw;
			}
		}
	}
}
