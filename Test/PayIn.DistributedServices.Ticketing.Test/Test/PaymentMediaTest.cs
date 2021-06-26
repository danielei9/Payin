using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Ticketing.Test
{
	[TestClass]
    public class PaymentMediaTest
    {
		[TestMethod]
		public async Task PaymentMediaCreateAsync()
		{
			using (var Server = new TicketingBusinessServer())
			{
				var now = DateTime.UtcNow;

				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var paymentConcessionId = paymentConcessions.FirstOrDefault()?.Id;
				Assert.IsNotNull(paymentConcessionId);

				// Get PaymentMedias
				var paymentMedias = await Server.PaymentMediaGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(paymentMedias);

				// Get PaymentMedia IFrame
				var paymentMediaIFrame = await Server.PaymentMediaCreateWebCardByUserAsync("notexists@pay-in.es", "Pruebas", "1234", "Bank");
				Assert.IsNotNull(paymentMediaIFrame);
				Assert.IsNotNull(paymentMediaIFrame.Arguments);
				Assert.AreNotEqual(paymentMediaIFrame.Request, "");
				Assert.AreNotEqual(paymentMediaIFrame.Verb, "");
				Assert.AreNotEqual(paymentMediaIFrame.Url, "");
				Assert.AreNotEqual(paymentMediaIFrame.Arguments.Count(), 0);

				// Get PaymentMedias2
				var paymentMedias2 = await Server.PaymentMediaGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(paymentMedias2);
				Assert.AreEqual(paymentMedias2.Count(), paymentMedias.Count() + 1);
			}
		}
    }
}
