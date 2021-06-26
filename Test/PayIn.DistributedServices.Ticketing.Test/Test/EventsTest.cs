using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.Common;
using PayIn.DistributedServices.Test.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Ticketing.Test
{
	[TestClass]
    public class EventsTest
    {
        [TestMethod]
        public async Task EventManagementAsync()
		{
			using (var Server = new TicketingBusinessServer())
			{
				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var paymentConcessionId = paymentConcessions.FirstOrDefault()?.Id;
				Assert.IsNotNull(paymentConcessionId);

				// Get Events
				var events = await Server.EventGetAllAsync(paymentConcessionId.Value);

				// Create Event
				var eventId = await Server.EventCreateAsync(
					paymentConcessionId.Value,
					null,
					null,
					"Lugar de conciertos",
					"Concierto",
					"Prueba EventManagementAsync",
					1000,
					new DateTime(2018, 7, 15, 22, 0, 0, DateTimeKind.Local),
					new DateTime(2018, 7, 15, 23, 59, 0, DateTimeKind.Local),
					new DateTime(2018, 7, 15, 21, 30, 0, DateTimeKind.Local),
					new DateTime(2018, 7, 15, 22, 30, 0, DateTimeKind.Local),
					1,
					null,
					"Benéfico",
					"Condiciones...",
					EventVisibility.Public);

				// Get Events
				var events2 = await Server.EventGetAllAsync(paymentConcessionId.Value);
				Assert.AreEqual(events.Count() + 1, events2.Count());

				// Get Event
				var event_ = await Server.EventGetAsync(eventId);
				Assert.IsNotNull(event_);
				Assert.AreEqual(event_.Place, "Lugar de conciertos");
				Assert.AreEqual(event_.Name, "Concierto");
				Assert.AreEqual(event_.Description, "Prueba EventManagementAsync");
				Assert.AreEqual(event_.Capacity, 1000);
				Assert.AreEqual(event_.EventStart.Value, new DateTime(2018, 7, 15, 22, 0, 0, DateTimeKind.Local));
				Assert.AreEqual(event_.EventEnd.Value, new DateTime(2018, 7, 15, 23, 59, 0, DateTimeKind.Local));
				Assert.AreEqual(event_.CheckInStart.Value, new DateTime(2018, 7, 15, 21, 30, 0, DateTimeKind.Local));
				Assert.AreEqual(event_.CheckInEnd.Value, new DateTime(2018, 7, 15, 22, 30, 0, DateTimeKind.Local));
				Assert.AreEqual(event_.EntranceSystemId, 1);
				Assert.AreEqual(event_.ShortDescription, "Benéfico");
				Assert.AreEqual(event_.Conditions, "Condiciones...");
				Assert.AreEqual(event_.Visibility, PayIn.Common.EventVisibility.Public);

				// Delete Event
				await Server.EventDeleteAsync(eventId);

				// Get Events
				var events3 = await Server.EventGetAllAsync(paymentConcessionId.Value);
				Assert.AreEqual(events.Count(), events3.Count());
			}
        }
    }
}
