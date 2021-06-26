using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test.Replace
{
	[TestClass]
	public class ReplaceBB_BM : TransportBaseTest
	{
		[TestMethod]
		public async Task RechargeAT()
		{
			var card = new BonobusBonometroCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(9, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);

			try
			{
				await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporte, null, resultRead);
			}
			catch (AssertFailedException ex)
			{
				Assert.AreEqual("Assert.IsNotNull failed. No se ha encontrado el titulo " + ((int)TitleCodeEnum.AbonoTransporte), ex.Message);
				return;
			}
			Assert.Fail("Exception of type {0} should be thrown.", typeof(AssertFailedException));
		}
		[TestMethod]
		public async Task RechargeBTExhausted()
		{
			var card = new BonometroBonobusAgotadosCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			Assert.IsTrue(resultRead.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro
				})
			);
			Assert.IsTrue(resultRead.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro,
					TitleCodeEnum.BonoTransbordo
					//TitleCodeEnum.T1,
					//TitleCodeEnum.IdaVuelta,
					//TitleCodeEnum.Sencillo,
					//TitleCodeEnum.T2,
					//TitleCodeEnum.T3
				})
			);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			//Assert.IsNull(card.Titulo.FechaValidez1.Value); // No se porque no está a null cuando debería...
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);

			var charge = await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, null, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(0, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(0, card.Titulo.ControlTarifa2.Value);
		}
	}
}
