using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test {
	[TestClass]
	public class ChargeAbono : TransportBaseTest
	{
		[TestMethod]
		public async Task Charge()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();
			var resultRead = await ReadCardAsync(card);

			var charge = await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporteJove, EigeZonaEnum.A, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporteJove, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(6, card.Titulo.ControlTarifa1.Value);
		}
	}
}
