using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test
{
	[TestClass]
	public class MarcelaTest : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var card = new MarcelaCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(9, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(0, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(0, card.Titulo.ControlTarifa2.Value);
			// Recharges
			Assert.IsTrue(result.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.BonoTransbordo
				})
			);
			// Charges
			Assert.IsTrue(result.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro,
					TitleCodeEnum.BonoTransbordo
					//TitleCodeEnum.Sencillo,
					//TitleCodeEnum.IdaVuelta,
					//TitleCodeEnum.T1,
					//TitleCodeEnum.T2,
					//TitleCodeEnum.T3
				})
			);
		}
		[TestMethod]
		public async Task RechargeBB()
		{
			var now = new DateTime(2016, 10, 18, 12, 0, 0);
			var card = new MarcelaCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(9, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(0, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(0, card.Titulo.ControlTarifa2.Value);

			await ChargeCardAsync(card, TitleCodeEnum.Bonobus, EigeZonaEnum.A, result);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(0, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(0, card.Titulo.ControlTarifa2.Value);
		}
	}
}
