using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace PayIn.Web
{
    public class WebRole : RoleEntryPoint
	{
		private TimeSpan Sleep = TimeSpan.FromMinutes(15);
        
		#region Run
		public override void Run()
		{
			Trace.TraceInformation("PayIn.Web entry point called");
#if DEBUG || TEST || HOMO || VILAMARXANT || FALLAS
			var localuri = new Uri(string.Format("http://{0}/mobile/main/v1/version", RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Http"].IPEndpoint));
#else
			var localuri = new Uri(string.Format("https://{0}/mobile/main/v1/version", RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Https"].IPEndpoint));
#endif // DEBUG || TEST || HOMO
			Trace.TraceInformation("HeartBeat url: {0}", localuri);

			while (true)
			{
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(localuri);
                    request.Method = "GET";
                    var response = request.GetResponse();
                    Trace.TraceInformation("HeartBeat response: {0}", response.ToString());
                }
                catch { }

                Thread.Sleep(Sleep);
				Trace.TraceInformation("Working");
			}
		}
		#endregion Run
	}
}