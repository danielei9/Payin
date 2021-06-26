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
	public class JacoboTest
	{
		[TestMethod]
		public async Task Jacobo1_DescuentoTaquilla_MemberAsync()
		{
			await ComprarEntradas("festero@pay-in.es", "Anticipada socio", 15.5M, 2);
		}
		[TestMethod]
		public async Task Jacobo1_DescuentoTaquilla_NoMemberAsync()
		{
			try
			{
				await ComprarEntradas("notexists@pay-in.es", "Anticipada socio", 15.5M, 2);
			}
			catch (ApplicationException e)
			{
				Assert.AreEqual("No se puede comprar un tipo de entrada no visible", e.Message);
			}
		}
		[TestMethod]
		public async Task Jacobo2_DescuentoTaquilla_MemberAsync()
		{
			await ComprarEntradaTaquilla("festero@pay-in.es", "Entradas socios", 3);
		}
		[TestMethod]
		public async Task Jacobo2_DescuentoTaquilla_NoMemberAsync()
		{
			try
			{
				await ComprarEntradaTaquilla("notexists@pay-in.es", "Entradas socios", 0);
			}
			catch (ApplicationException e)
			{
				Assert.AreEqual("No se puede comprar un tipo de entrada no visible", e.Message);
			}
		}
		[TestMethod]
		public async Task Jacobo3_DescuentoBarra_MemberAsync()
		{
			await ComprarProductoDescuentoBarra("festero@pay-in.es", "Compra en barra", 3);

			await ComprarProductos(
				"festero@pay-in.es",
				"festero@pay-in.es",
				new DateTime(2017, 10, 1, 23, 0, 0, DateTimeKind.Local),
				1, // Quantity
				39, // Gintonic
				14, // 15€ - 1€
				2 // Producto y descuento
			);
		}
		[TestMethod]
		public async Task Jacobo3_DescuentoBarra_Member2Async()
		{
			await ComprarProductoDescuentoBarra("festero@pay-in.es", "Compra en barra", 3);

			await ComprarProductos(
				"festero@pay-in.es",
				"festero@pay-in.es",
				new DateTime(2017, 10, 1, 23, 0, 0, DateTimeKind.Local),
				2, // Quantity
				39, // Gintonic
				28, // 2 * (15€ - 1€)
				2 // Producto y descuento
			);
		}
		[TestMethod]
		public async Task Jacobo3_DescuentoBarra_MemberOutTimeAsync()
		{
			await ComprarProductoDescuentoBarra("festero@pay-in.es", "Compra en barra", 3);

			await ComprarProductos(
				"festero@pay-in.es",
				"festero@pay-in.es",
				new DateTime(2017, 10, 1, 14, 0, 0, DateTimeKind.Local),
				1, // Quantity
				39, // Gintonic
				15, // 15€
				1 // Producto
			);
		}
		[TestMethod]
		public async Task Jacobo3_DescuentoBarra_NoMemberAsync()
		{
			try
			{
				await ComprarProductoDescuentoBarra("notexists@pay-in.es", "Compra en barra", 0);
			}
			catch (ApplicationException e)
			{
				Assert.AreEqual("No se puede comprar un tipo de entrada no visible", e.Message);
			}

			await ComprarProductos(
				"notexists@pay-in.es",
				"notexists@pay-in.es",
				new DateTime(2017, 10, 1, 23, 0, 0, DateTimeKind.Local),
				1, // Quantity
				39, // Gintonic
				15, // 15€
				1 // Producto
			);
		}
		[TestMethod]
		public async Task Jacobo4_DescuentoMerchandaising_MemberAsync()
		{
			await ComprarProductoDescuentoMerchandaising("festero@pay-in.es", "Compra merchandaising", 3);

			await ComprarProductos(
				"festero@pay-in.es",
				"festero@pay-in.es",
				new DateTime(2017, 10, 1, 23, 0, 0, DateTimeKind.Local),
				1, // Quantity
				41, // Pin
				0.75M, // 1€ - 25%
				2 // Producto + Descuento
			);
		}
		[TestMethod]
		public async Task Jacobo4_DescuentoMerchandaising_NoMemberAsync()
		{
			try
			{
				await ComprarProductoDescuentoMerchandaising("notexists@pay-in.es", "Compra merchandaising", 0);
			}
			catch (ApplicationException e)
			{
				Assert.AreEqual("No se puede comprar un tipo de entrada no visible", e.Message);
			}

			await ComprarProductos(
				"notexists@pay-in.es",
				"notexists@pay-in.es",
				new DateTime(2017, 10, 1, 23, 0, 0, DateTimeKind.Local),
				1, // Quantity
				41, // Pin
				1, // 1€
				1 // Producto
			);
		}

		#region ComprarEntradaTaquilla
		private async Task ComprarEntradaTaquilla(string login, string campaignName, int expectedCampaings)
		{
			using (var Server = new TicketingBusinessServer())
			{
				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var club = paymentConcessions
					.Where(x => x.Value == "Club")
					.FirstOrDefault();
				Assert.IsNotNull(club);
				var clubManager = paymentConcessions
					.Where(x => x.Value == "Club manager")
					.FirstOrDefault();
				Assert.IsNotNull(clubManager);

				// Get Events
				var events = await Server.EventGetAllAsync(club.Id);
				var event_ = events
					.Where(x => x.Name == "Pruebas Jacobo")
					.FirstOrDefault();
				Assert.IsNotNull(event_);

				// Get Campaigns
				var campaigns = await Server.CampaignsGetByUserAsync(login, event_.Id);
				Assert.AreEqual(expectedCampaings, campaigns.Count());
				if (expectedCampaings == 0)
					return;

				// Get Campaign
				var campaign = campaigns
					.Where(x => x.Title == campaignName)
					.FirstOrDefault();
				Assert.IsNotNull(campaign);
				Assert.AreEqual(campaignName, campaign.Title);
				Assert.AreEqual("pay[in]/promo:{{\"id\":\"J\",\"card\":\"A\",\"eventId\":\"NJ\",\"caducity\":\"F4BOSYPM\",\"type\":\"D\",\"quantity\":\"A\",\"entrances\":\"OA,OB\"}}", campaign.Qr);
			}
		}
		#endregion ComprarEntradaTaquilla

		#region ComprarProductoDescuentoBarra
		private async Task ComprarProductoDescuentoBarra(string login, string campaignName, int expectedCampaings)
		{
			using (var Server = new TicketingBusinessServer())
			{
				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var club = paymentConcessions
					.Where(x => x.Value == "Club")
					.FirstOrDefault();
				Assert.IsNotNull(club);
				var clubManager = paymentConcessions
					.Where(x => x.Value == "Club manager")
					.FirstOrDefault();
				Assert.IsNotNull(clubManager);

				// Get Events
				var events = await Server.EventGetAllAsync(club.Id);
				var event_ = events
					.Where(x => x.Name == "Pruebas Jacobo")
					.FirstOrDefault();
				Assert.IsNotNull(event_);

				// Get Campaigns
				var campaigns = await Server.CampaignsGetByUserAsync(login, event_.Id);
				Assert.AreEqual(expectedCampaings, campaigns.Count());
				if (expectedCampaings == 0)
					return;

				// Get Campaign
				var campaign = campaigns
					.Where(x => x.Title == campaignName)
					.FirstOrDefault();
				Assert.IsNotNull(campaign);
				Assert.AreEqual(campaignName, campaign.Title);
				Assert.AreEqual("pay[in]/promo:{{\"id\":\"L\",\"since\":\"B3M\",\"until\":\"CI2\",\"card\":\"A\",\"eventId\":\"NJ\",\"caducity\":\"F4BOSYPM\",\"type\":\"D\",\"quantity\":\"DE\",\"products\":\"BH,BI\"}}", campaign.Qr);
			}
		}
		#endregion ComprarProductoDescuentoBarra

		#region ComprarProductoDescuentoMerchandaising
		private async Task ComprarProductoDescuentoMerchandaising(string login, string campaignName, int expectedCampaings)
		{
			using (var Server = new TicketingBusinessServer())
			{
				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var club = paymentConcessions
					.Where(x => x.Value == "Club")
					.FirstOrDefault();
				Assert.IsNotNull(club);
				var clubManager = paymentConcessions
					.Where(x => x.Value == "Club manager")
					.FirstOrDefault();
				Assert.IsNotNull(clubManager);

				// Get Events
				var events = await Server.EventGetAllAsync(club.Id);
				var event_ = events
					.Where(x => x.Name == "Pruebas Jacobo")
					.FirstOrDefault();
				Assert.IsNotNull(event_);

				// Get Campaigns
				var campaigns = await Server.CampaignsGetByUserAsync(login, event_.Id);
				Assert.AreEqual(expectedCampaings, campaigns.Count());
				if (expectedCampaings == 0)
					return;

				// Get Campaign
				var campaign = campaigns
					.Where(x => x.Title == campaignName)
					.FirstOrDefault();
				Assert.IsNotNull(campaign);
				Assert.AreEqual(campaignName, campaign.Title);
				Assert.AreEqual("pay[in]/promo:{{\"id\":\"M\",\"card\":\"A\",\"eventId\":\"NJ\",\"caducity\":\"F4BOSYPM\",\"type\":\"G\",\"quantity\":\"Z\",\"products\":\"BJ,BK\"}}", campaign.Qr);
			}
		}
		#endregion ComprarProductoDescuentoMerchandaising

		#region ComprarEntradas
		private async Task ComprarEntradas(string email, string entranceTypeName, decimal expectedTotal, int expectedLines)
		{
			using (var Server = new TicketingBusinessServer())
			{
				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var club = paymentConcessions
					.Where(x => x.Value == "Club")
					.FirstOrDefault();
				Assert.IsNotNull(club);
				var clubManager = paymentConcessions
					.Where(x => x.Value == "Club manager")
					.FirstOrDefault();
				Assert.IsNotNull(clubManager);

				// Get Events
				var events = await Server.EventGetAllAsync(club.Id);
				var event_ = events
					.Where(x => x.Name == "Pruebas Jacobo")
					.FirstOrDefault();
				Assert.IsNotNull(event_);

				// Get EntranceTypes
				var entranceTypes = await Server.EntranceTypeGetAllAsync(event_.Id);
				var entranceType = entranceTypes
					.Where(x => x.Name == entranceTypeName)
					.FirstOrDefault();
				Assert.IsNotNull(entranceType);

				// Create Ticket
				var tickets = await Server.TicketCreateEntrancesAsync(
					"",
					clubManager.Id,
					new List<PublicTicketCreateEntrancesAndGetArguments_TicketLine>
					{
					new PublicTicketCreateEntrancesAndGetArguments_TicketLine
					{
						EntranceTypeId = entranceType.Id,
						Quantity = 1
					}
					},
					TicketType.Ticket,
					email,
					email,
                    DateTime.UtcNow // Probar 05022018
                );
				var ticket = tickets.Data.FirstOrDefault();
				Assert.IsNotNull(ticket);
				Assert.AreEqual(expectedTotal, ticket.Amount);

				var lines = ticket.Lines;
				Assert.AreEqual(lines.Count(), expectedLines);

				var paymentMedias = tickets.PaymentMedias;
				Assert.IsNotNull(paymentMedias);
				Assert.AreNotEqual(paymentMedias.Count(), 0);

				// Pay ticket
				var payment = await Server.TicketPayUserAsync(ticket.Id, email, "XaviPaper", "12345678A", "Xavier Jorge Cerdá", "C/Libertad", paymentMedias.FirstOrDefault().Id, "1234", ticket.Amount);

				// Get entrances
				var entrances = ticket.Lines
					.SelectMany(x => x.Entrances);

				Assert.IsNotNull(entrances);
				Assert.AreEqual(entrances.Count(), 1);
			}
		}
		#endregion ComprarEntradas

		#region ComprarProductos
		private async Task ComprarProductos(string email, string login, DateTime now, int quantity, int productId, decimal expectedTotal, int expectedLines)
		{
			using (var Server = new TicketingBusinessServer())
			{
				await Server.LoginPaymentApiAsync();

				// Get PaymentConcessions
				var paymentConcessions = await Server.PaymentConcessionGetSelectorAsync();
				var club = paymentConcessions
					.Where(x => x.Value == "Club")
					.FirstOrDefault();
				Assert.IsNotNull(club);
				var clubManager = paymentConcessions
					.Where(x => x.Value == "Club manager")
					.FirstOrDefault();
				Assert.IsNotNull(clubManager);

				// Create Ticket
				var tickets = await Server.TicketCreateProductsAsync(
					"",
					club.Id,
					new List<PublicTicketCreateProductsAndGetArguments_TicketLine>
					{
					new PublicTicketCreateProductsAndGetArguments_TicketLine
					{
						ProductId = productId,
						Quantity = quantity
					}
					},
					TicketType.Ticket,
					email,
					login,
					now
				);
				var ticket = tickets.Data.FirstOrDefault();
				Assert.IsNotNull(ticket);
				Assert.AreEqual(expectedTotal, ticket.Amount);

				var lines = ticket.Lines;
				Assert.AreEqual(lines.Count(), expectedLines);

				var paymentMedias = tickets.PaymentMedias;
				Assert.IsNotNull(paymentMedias);
				Assert.AreNotEqual(paymentMedias.Count(), 0);

				// Pay ticket
				var payment = await Server.TicketPayUserAsync(ticket.Id, login, "XaviPaper", "12345678A", "Xavier Jorge Cerdá", "C/Libertad", paymentMedias.FirstOrDefault().Id, "1234", ticket.Amount);
				Assert.IsNotNull(payment);
			}
		}
		#endregion ComprarProductos
	}
}
