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
	public class ReadAbonoTJoveAmpliadoTest : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var card = new AbonoTJoveAmpliadoCard();
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
					TitleCodeEnum.AbonoTransporteJove,
					TitleCodeEnum.AbonoTransporte,
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
			Assert.AreEqual(new DateTime(2016, 7, 30, 8, 57, 0), card.Titulo.FechaValidez1.Value);
			Assert.IsTrue(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(6, card.Titulo.ControlTarifa1.Value);
		}
	}	
}
