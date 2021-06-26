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
	public class BonometroBonobusAgotadosTest : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var card = new BonometroBonobusAgotadosCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			var recharges = result.Recharges;
			Assert.IsTrue(recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro
				})
			);
			var charges = result.Charges;
			Assert.IsTrue(charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro,
					TitleCodeEnum.BonoTransbordo
					//TitleCodeEnum.T1,
					//TitleCodeEnum.Sencillo,
					//TitleCodeEnum.IdaVuelta,
					//TitleCodeEnum.T2,
					//TitleCodeEnum.T3
				})
			);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);
		}
		[TestMethod]
		public async Task RechargeBonoTransbordo()
		{
			var card = new BonometroBonobusAgotadosCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			var resultCharge = await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, resultRead);
			Assert.IsTrue(resultCharge.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.BonoTransbordo
				})
			);
			Assert.IsTrue(resultCharge.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
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
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(0, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(0, card.Titulo.ControlTarifa2.Value);
		}
		[TestMethod]
		public async Task RechargeBonometro_BonoTransbordo()
		{
			var now = new DateTime(2016, 10, 16, 10, 0, 0);
			var card = new BonometroBonobusAgotadosCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);

			// Recarga Bono bonometro
			result = await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A, result, now);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);

			// Recarga Bono transbordo
			result = await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result, now);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(0, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(0, card.Titulo.ControlTarifa2.Value);
		}
		[TestMethod]
		public async Task RechargeBonobus_BonoTransbordo()
		{
			var now = new DateTime(2016, 10, 16, 10, 0, 0);
			var card = new BonometroBonobusAgotadosCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card, now);

			// Recarga Bonobus
			result = await ChargeCardAsync(card, TitleCodeEnum.Bonobus, EigeZonaEnum.A, result, now);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);

			// Recarga Bono transbordo
			try
			{
				result = await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result, now);
			}
			catch(AssertFailedException ex)
			{
				Assert.AreEqual("Assert.IsNotNull failed. No se ha encontrado el titulo " + ((int)TitleCodeEnum.BonoTransbordo), ex.Message);
				return;
			}
			Assert.Fail("Exception of type {0} should be thrown.", typeof(AssertFailedException));
		}
	}
}
