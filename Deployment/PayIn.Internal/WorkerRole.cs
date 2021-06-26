using Microsoft.WindowsAzure.ServiceRuntime;
using PayIn.Common.DI.Internal;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Deployment.Internal
{
	public class WorkerRole : RoleEntryPoint
	{
		private TimeSpan Sleep = TimeSpan.FromMinutes(10);

		#region OnStart
		public override bool OnStart()
		{
			ServicePointManager.DefaultConnectionLimit = 12;
			return base.OnStart();
		}
		#endregion OnStart

		#region Run
		public override void Run()
		{
			Trace.TraceInformation("PayIn.Deployment.Internal entry point called");

			while (true)
			{
				Thread.Sleep(Sleep);

				Trace.TraceInformation("Working");
			}
		}
		#endregion Run
	}
}
