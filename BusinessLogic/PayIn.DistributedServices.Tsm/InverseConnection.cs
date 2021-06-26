using Microsoft.AspNet.SignalR;
using System;
using Xp.Application;

namespace PayIn.DistributedServices.Tsm
{
	public class InverseConnection : IInverseConnection
	{
		public InverseConnection(IConnection connection)
		{
		}

		public T Send<T>(object args)
		{
			throw new NotImplementedException();
		}
	}
}
