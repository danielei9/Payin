using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.DistributedServices.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Ticketing.Test
{
	[TestClass]
    public class TicketTest
    {
		[TestMethod]
		public async Task TicketCreateEntranceAsync()
		{
			using (var Server = new TicketingBusinessServer())
			{
                var now = new DateTime(2018, 6, 15);

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

				// Get EntranceType
				var entranceType = await Server.EntranceTypeGetAsync(entranceTypeId);
				Assert.IsNotNull(entranceType);

				// Get Entrances
				var entrances = await Server.EntranceGetByUserAsync("notexists@pay-in.es");

				// Create Ticket
				var all = await Server.TicketCreateEntrancesAsync(
					"reference",
					paymentConcessionId.Value,
					new List<PublicTicketCreateEntrancesAndGetArguments_TicketLine>
					{
					    new PublicTicketCreateEntrancesAndGetArguments_TicketLine
					    {
						    EntranceTypeId = entranceTypeId,
						    Quantity = 1
					    }
					},
					TicketType.Ticket,
					"notexists@pay-in.es",
					"notexists@pay-in.es",
                    now
				);
				var ticket = all.Data.FirstOrDefault();
				Assert.IsNotNull(ticket);
				Assert.AreEqual(ticket.Lines.Count(), 2);
				var paymentMedias = all.PaymentMedias;
				Assert.IsNotNull(paymentMedias);
				Assert.AreNotEqual(paymentMedias.Count(), 0);

				// Get entrances
				var entrances3 = await Server.EntranceGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(entrances3);
				Assert.AreEqual(entrances.Count(), entrances3.Count());

				// Pay ticket
				var payment = await Server.TicketPayUserAsync(ticket.Id, "notexists@pay-in.es", "XaviPaper", "12345678A", "Xavier Jorge Cerdá", "C/Libertad", paymentMedias.FirstOrDefault().Id, "1234", ticket.Amount);

				// Get entrances
				var entrances2 = await Server.EntranceGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(entrances2);
				Assert.AreEqual(entrances.Count() + 1, entrances2.Count());
			}
		}
		[TestMethod]
		public async Task TicketCreateEntranceMinInfoAsync()
        {
            var now = new DateTime(2018, 6, 15);

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

				// Get EntranceType
				var entranceType = await Server.EntranceTypeGetAsync(entranceTypeId);
				Assert.IsNotNull(entranceType);

				// Get Entrances
				var entrances = await Server.EntranceGetByUserAsync("notexists@pay-in.es");

				// Create Ticket
				var all = await Server.TicketCreateEntrancesAsync(
					"reference",
					paymentConcessionId.Value,
					new List<PublicTicketCreateEntrancesAndGetArguments_TicketLine>
					{
					new PublicTicketCreateEntrancesAndGetArguments_TicketLine
					{
						EntranceTypeId = entranceTypeId,
						Quantity = 1
					}
					},
					TicketType.Ticket,
					null,
					"notexists@pay-in.es",
                    now
				);
				var ticket = all.Data.FirstOrDefault();
				Assert.IsNotNull(ticket);
				Assert.AreEqual(ticket.Lines.Count(), 2);
				var paymentMedias = all.PaymentMedias;
				Assert.IsNotNull(paymentMedias);
				Assert.AreNotEqual(paymentMedias.Count(), 0);

				// Get entrances
				var entrances3 = await Server.EntranceGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(entrances3);
				Assert.AreEqual(entrances.Count(), entrances3.Count());

				// Pay ticket
				var payment = await Server.TicketPayUserAsync(ticket.Id, "notexists@pay-in.es", "XaviPaper", "12345678A", "Xavier Jorge Cerdá", "C/Libertad", paymentMedias.FirstOrDefault().Id, "1234", ticket.Amount);

				// Get entrances
				var entrances2 = await Server.EntranceGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(entrances2);
				Assert.AreEqual(entrances.Count() + 1, entrances2.Count());
			}
		}
		[TestMethod]
		public async Task TicketCreateFreeEntranceAsync()
        {
            var now = new DateTime(2018, 6, 15);

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

				// Create EntranceType
				var entranceTypeId = await Server.EntranceTypeCreateAsync(
					eventId,
					"Gratuita",
					"Entrada gratuita ...",
					0,
					100,
					new DateTime(2018, 6, 1, 0, 0, 0, DateTimeKind.Local),
					new DateTime(2018, 6, 30, 0, 0, 0, DateTimeKind.Local),
					new DateTime(2018, 7, 15, 21, 45, 0, DateTimeKind.Local),
					new DateTime(2018, 7, 15, 22, 15, 0, DateTimeKind.Local),
					0,
					null,
					null,
					1,
					"Prueba EntranceGratisTypeManagementAsync",
					"Condiciones...",
					1,
					null,
					null
				);

				// Get EntranceType
				var entranceType = await Server.EntranceTypeGetAsync(entranceTypeId);
				Assert.IsNotNull(entranceType);

				// Get Entrances
				var entrances = await Server.EntranceGetByUserAsync("notexists@pay-in.es");

				// Create Ticket
				var all = await Server.TicketCreateEntrancesAsync(
					"reference",
					paymentConcessionId.Value,
					new List<PublicTicketCreateEntrancesAndGetArguments_TicketLine>
					{
					new PublicTicketCreateEntrancesAndGetArguments_TicketLine
					{
						EntranceTypeId = entranceTypeId,
						Quantity = 1
					}
					},
					TicketType.Ticket,
					"notexists@pay-in.es",
					"notexists@pay-in.es",
                    now
                );
				var ticket = all.Data.FirstOrDefault();
				Assert.IsNotNull(ticket);
				Assert.AreEqual(ticket.Lines.Count(), 1);
				Assert.AreEqual(ticket.Amount, 0);
				var paymentMedias = all.PaymentMedias;
				Assert.IsNotNull(paymentMedias);
				Assert.AreNotEqual(paymentMedias.Count(), 0);

				// Get entrances
				var entrances2 = await Server.EntranceGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(entrances2);
				Assert.AreEqual(entrances.Count() + 1, entrances2.Count());
			}
		}
		[TestMethod]
		public async Task TicketPayEntranceAndCreateWebCardAsync()
        {
            var now = new DateTime(2018, 6, 15);

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

				// Get EntranceType
				var entranceType = await Server.EntranceTypeGetAsync(entranceTypeId);
				Assert.IsNotNull(entranceType);

				// Get Entrances
				var entrances = await Server.EntranceGetByUserAsync("notexists@pay-in.es");

				// Create Ticket
				var ticketCreateResponse = await Server.TicketCreateEntrancesAsync(
					"reference",
					paymentConcessionId.Value,
					new List<PublicTicketCreateEntrancesAndGetArguments_TicketLine>
					{
					new PublicTicketCreateEntrancesAndGetArguments_TicketLine
					{
						EntranceTypeId = entranceTypeId,
						Quantity = 1
					}
					},
					TicketType.Ticket,
					"notexists@pay-in.es",
					"notexists@pay-in.es",
                    now
                );
				var ticket = ticketCreateResponse.Data.FirstOrDefault();
				Assert.IsNotNull(ticket);
				Assert.AreEqual(ticket.Lines.Count(), 2);

				// Get entrances
				var entrances3 = await Server.EntranceGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(entrances3);
				Assert.AreEqual(entrances.Count(), entrances3.Count());

				// Pagar ticket con nueva tarjeta
				var all = await Server.TicketPayAndCreateWebCardByUser(
					ticketId: ticket.Id,
					userEmail: "notexists@pay-in.es",
					userTaxNumber: "",
					userName: "Pruebas",
					userTaxName: "Pruebas",
					userTaxLastName: "de Compra con tarjeta nueva",
					userTaxAddress: "",
					userBirthday: null,
					userPhone: "",
					login: "notexists@pay-in.es",
					pin: "1234",
					name: null, //"Pruebas",
					bankEntity: null //"Bank"
				);

				// Get entrances
				var entrances2 = await Server.EntranceGetByUserAsync("notexists@pay-in.es");
				Assert.IsNotNull(entrances2);
				Assert.AreEqual(entrances.Count(), entrances2.Count());
				// Hay que tener en cuenta que como no se ejecuta el IFrame, no se va a activar la entrada, ...
			}
		}
	}
}
