using System;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test.Helpers
{
	[Synchronization]
	public class BaseServer
	{
		#region LoginWebAsync
		public async Task<HttpServer> LoginWebAsync()
		{
			var server = new HttpServer("superadministrator@pay-in.es", "Pa$$w0rd", "PayInWebApp", null);
			return server;
		}
		#endregion LoginWebAsync

		#region LoginAndroidAsync
		public async Task<HttpServer> LoginAndroidAsync()
		{
			var server = new HttpServer("user@pay-in.es", "Pa$$w0rd", "PayInAndroidNativeApp", "PayInAndroidNativeApp@123456");
			return server;
		}
        #endregion LoginAndroidAsync

        #region LoginPaymentApiAsync
        public async Task<HttpServer> LoginPaymentApiAsync()
		{
			var server = new HttpServer("clubmanager@pay-in.es", "Pa$$w0rd", "PayInPaymentApi", "PayInPayment@1912");
			await server.GetTokenAsync();
			return server;
		}
		#endregion LoginPaymentApiAsync
	}
}