using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test.RechargeAbono
{
	[TestClass]
	public class AbonoTJoveAmpliadoTest : TransportBaseTest
	{
		[TestMethod]
		public async Task RechargeAgotado()
		{
			var card = new AbonoTJoveAmpliadoCard();
			await Server.LoginAndroidAsync();
			var resultRead = await ReadCardAsync(card);

			var charge = await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporteJove, EigeZonaEnum.A, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporteJove, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsTrue(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(6, card.Titulo.ControlTarifa1.Value);
		}
		/// <summary>
		/// Recarga de un abono activo y ampliado
		/// No se puede recargar
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task RechargeActivo()
		{
			var now = new DateTime(2016, 7, 20, 8, 57, 0);

			var card = new AbonoTJoveAmpliadoCard();
			await Server.LoginAndroidAsync();
			var resultRead = await ReadCardAsync(card);

			try
			{
				await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporteJove, EigeZonaEnum.A, resultRead, now);
			}
			catch (ApplicationException ex)
			{
				Assert.AreEqual("De momento no podemos recargar este título, inténtalo con otro", ex.Message);
				return;
			}
			Assert.Fail("Exception of type {0} should be thrown.", typeof(ApplicationException));



			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporteJove, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsTrue(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(6, card.Titulo.ControlTarifa1.Value);
		}
	}
}
