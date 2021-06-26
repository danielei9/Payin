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
	public class AbonoTransporteTest : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var card = new AbonoCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);

		    var recharges = result.Recharges;
			Assert.IsTrue(recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.AbonoTransporteJove
				})
			);
			var charges = result.Charges;
			Assert.IsTrue(charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.AbonoTransporte,
					TitleCodeEnum.AbonoTransporteJove,
					TitleCodeEnum.BonoTransbordo,
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro
					//TitleCodeEnum.Sencillo,
					//TitleCodeEnum.IdaVuelta,
					//TitleCodeEnum.T1,
					//TitleCodeEnum.T2,
					//TitleCodeEnum.T3
				})
			);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporteJove, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
		}

		[TestMethod]
		public async Task ChargeATFromBT()
		{
			var now = new DateTime(2016, 1, 1);

			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			var charge = await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, resultRead);

			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);

			await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporte, EigeZonaEnum.A | EigeZonaEnum.B, charge);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporte, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			//Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			//Assert.AreEqual(6, card.Titulo.SaldoViaje1.Value);
			//Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
		}
	}
}
