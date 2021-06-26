using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.Common;
using PayIn.DistributedServices.Test.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Ticketing.Test
{
	[TestClass]
    public class EntranceTypeTest
    {
		[TestMethod]
        public async Task EntranceTypeManagementAsync()
		{
			using (var Server = new TicketingBusinessServer())
			{
				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var paymentConcessionId = paymentConcessions.FirstOrDefault()?.Id;
				Assert.IsNotNull(paymentConcessionId);

				// Create Event
				var eventId = await Server.EventCreateAsync(paymentConcessionId.Value, null, null,
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
					EventVisibility.Public
				);

				// Get EntranceTypes
				var entranceTypes = await Server.EntranceTypeGetAllAsync(eventId);

				// Create EntranceType
				var entranceTypeId = await Server.EntranceTypeCreateAsync(
					eventId,
					"General",
					"Entrada general ...",
					10.0M,
					100,
					new DateTime(2018, 6, 1, 0, 0, 0, DateTimeKind.Local),
					new DateTime(2018, 6, 30, 0, 0, 0, DateTimeKind.Local),
					new DateTime(2018, 7, 15, 21, 45, 0, DateTimeKind.Local),
					new DateTime(2018, 7, 15, 22, 15, 0, DateTimeKind.Local),
					1M,
					null,
					null,
					1,
					"Prueba EntranceTypeManagementAsync",
					"Condiciones...",
					1,
					null,
					null
				);

				// Get EntranceTypes
				var entranceTypes2 = await Server.EntranceTypeGetAllAsync(eventId);
				Assert.AreEqual(entranceTypes.Count() + 1, entranceTypes2.Count());

				// Get EntranceType
				var entranceType = await Server.EntranceTypeGetAsync(entranceTypeId);
				Assert.IsNotNull(entranceType);
				Assert.AreEqual(entranceType.Id, entranceTypeId);
				Assert.AreEqual(entranceType.IsVisible, true);
				Assert.AreEqual(entranceType.Name, "General");
				Assert.AreEqual(entranceType.State, EntranceTypeState.Active);
				Assert.AreEqual(entranceType.Description, "Entrada general ...");
				Assert.AreEqual(entranceType.Price, 10);
				Assert.AreEqual(entranceType.PhotoUrl, "");
				Assert.AreEqual(entranceType.MaxEntrance, 100);
				Assert.AreEqual(entranceType.SellStart.Value, new DateTime(2018, 6, 1, 0, 0, 0, DateTimeKind.Local));
				Assert.AreEqual(entranceType.SellEnd.Value, new DateTime(2018, 6, 30, 0, 0, 0, DateTimeKind.Local));
				Assert.AreEqual(entranceType.CheckInStart.Value, new DateTime(2018, 7, 15, 21, 45, 0, DateTimeKind.Local));
				Assert.AreEqual(entranceType.CheckInEnd.Value, new DateTime(2018, 7, 15, 22, 15, 0, DateTimeKind.Local));
				Assert.AreEqual(entranceType.ExtraPrice, 1);
				Assert.AreEqual(entranceType.EventId, eventId);
				Assert.AreEqual(entranceType.EventName, "Concierto");
				Assert.IsNull(entranceType.RangeMin);
				Assert.IsNull(entranceType.RangeMax);
				Assert.AreEqual(entranceType.ShortDescription, "Prueba EntranceTypeManagementAsync");
				Assert.AreEqual(entranceType.Conditions, "Condiciones...");
				Assert.AreEqual(entranceType.MaxSendingCount, 1);
				Assert.AreEqual(entranceType.NumDays, 1);
				Assert.AreEqual(entranceType.StartDay, null);
				Assert.AreEqual(entranceType.EndDay, null);

				// Delete EntranceType
				await Server.EntranceTypeDeleteAsync(entranceTypeId);

				// Get EntranceTypes
				var entranceTypes3 = await Server.EntranceTypeGetAllAsync(eventId);
				Assert.AreEqual(entranceTypes.Count(), entranceTypes3.Count());

				// Delete Event
				await Server.EventDeleteAsync(eventId);
			}
        }
    }
}
