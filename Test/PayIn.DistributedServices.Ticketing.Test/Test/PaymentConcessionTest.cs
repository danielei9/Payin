using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Ticketing.Test
{
	[TestClass]
    public class PaymentConcessionTest
    {
        [TestMethod]
        public async Task PaymentConcessionGetSelectorAsync()
		{
			using (var Server = new TicketingBusinessServer())
			{
				var now = DateTime.UtcNow;

				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var paymentConcessionId = paymentConcessions.FirstOrDefault()?.Id;
				Assert.IsNotNull(paymentConcessionId);
			}
        }
    }
}
