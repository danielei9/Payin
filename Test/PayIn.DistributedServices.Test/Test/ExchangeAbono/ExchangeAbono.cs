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
	public class ExchangeAbono : TransportBaseTest
	{

		[TestMethod]
		public async Task ExchangeA_AB()
		{
			var card = new BonoTransbordo30VCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(30, card.Titulo.SaldoViaje1.Value);
			/*Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);

			await ChargeCardAsync(card, 1003, EigeZonaEnum.A | EigeZonaEnum.B, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(18, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);*/
		}
	}
}
