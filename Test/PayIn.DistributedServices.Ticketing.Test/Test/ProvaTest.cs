using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using PayIn.Common;
using PayIn.DistributedServices.Test.Helpers;
using PayIn.DistributedServices.Common.Test;

namespace PayIn.DistributedServices.Ticketing.Test
{
    [TestClass]
    public class ProvaTest
    {
		BaseServer Server = new BaseServer();

		[TestMethod]
        public async Task ProvaAsync()
        {
   //         // Get PaymentConcessions
			//var items1 = await Server.GetAsync<ResultBase<SelectorResult>>(
			//	"/public/paymentconcession/v1/selector"
			//);

			//// Get PaymentConcessions
			//await Task.Delay(TimeSpan.FromMinutes(1));
			//var items2 = await Server.GetAsync<ResultBase<SelectorResult>>(
			//	"/public/paymentconcession/v1/selector"
			//);

			//// Get PaymentConcessions
			//await Task.Delay(TimeSpan.FromMinutes(30));
			//var items3 = await Server.GetAsync<ResultBase<SelectorResult>>(
			//	"/public/paymentconcession/v1/selector"
			//);
		}
    }
}
