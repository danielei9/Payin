using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test.Replace
{
	[TestClass]
	public class ReplaceBM : TransportBaseTest
	{
		[TestMethod]
		public async Task RechargeAT()
		{
			var card = new BonometroCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);

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

		public async Task RechargeTATMensual()
		{
			var card = new BonometroCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);

			try
			{
				await ChargeCardAsync(card, TitleCodeEnum.TatMensual, null, resultRead);
			}
			catch (AssertFailedException ex)
			{
				Assert.AreEqual("Assert.IsNotNull failed. No se ha encontrado el titulo " + ((int)TitleCodeEnum.TatMensual), ex.Message);
				return;
			}
			Assert.Fail("Exception of type {0} should be thrown.", typeof(AssertFailedException));
		}
	}
}
