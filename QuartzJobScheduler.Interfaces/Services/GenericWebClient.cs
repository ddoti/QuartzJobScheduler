﻿using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace QuartzJobScheduler
{
	public class GenericWebClient<T> : ClientBase<T> where T : class
	{
		public GenericWebClient()
		{
		}

		public GenericWebClient(string endpointConfigurationName)
			: base(endpointConfigurationName)
		{
		}

		public GenericWebClient(string endpointConfigurationName, string remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		public GenericWebClient(string endpointConfigurationName, EndpointAddress remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		public GenericWebClient(Binding binding, EndpointAddress remoteAddress)
			: base(binding, remoteAddress)
		{
		}

		public T Proxy
		{
			get
			{
				return Channel;
			}
		}
	}
}
