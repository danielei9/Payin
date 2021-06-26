using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test {  
	[TestClass]
	public class ExchangeBono : TransportBaseTest
	{
	
		[TestMethod]
		public async Task ExchangeA_AB()
		{
			var card = new BonometroCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);

			await ChargeCardAsync(card, 1003, EigeZonaEnum.A | EigeZonaEnum.B, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(18, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
		}
		[TestMethod]
		public async Task ExchangeA_ABC()
		{
			var card = new BonometroCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);

			await ChargeCardAsync(card, 1003, EigeZonaEnum.A | EigeZonaEnum.B | EigeZonaEnum.C, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(18, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
		}
		[TestMethod]
		public async Task ExchangeB_AB()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			await ChargeCardAsync(card, 1003, EigeZonaEnum.B, resultRead);
			var resultRead2 = await ReadCardAsync(card);
			await ChargeCardAsync(card, 1003, EigeZonaEnum.A | EigeZonaEnum.B, resultRead2);

			
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(20, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
		}
		[TestMethod]
		public async Task ExchangeAB_A()
		{
			var now = new DateTime(2016, 10, 16, 10, 0, 0);
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card, now);

			await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A | EigeZonaEnum.B, resultRead, now);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			Assert.AreEqual(EigeZonaEnum.A | EigeZonaEnum.B, card.Titulo.ValidezZonal1.Value);

			var resultRead2 = await ReadCardAsync(card, now);
			var title = resultRead2.Charges
				.Where(x => x.Value<int>("code") == (int)TitleCodeEnum.Bonometro)
				.FirstOrDefault();
			Assert.IsNotNull(title);
			var price = title.Value<JArray>("prices")
				.Where(x => x.Value<int>("zone") == (int)EigeZonaEnum.A)
				.FirstOrDefault();
			Assert.IsNotNull(price);
			Assert.AreEqual((int)RechargeType.Replace, price.Value<int>("rechargeType"));
			Assert.AreEqual(0, price.Value<int>("slot"));

			await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A, resultRead2, now);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
		}
	}
}
