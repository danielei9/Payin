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
	public class Marcela4Test : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var now = new DateTime(2016, 10, 27);
			var card = new Marcela4Card();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card, now);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(new DateTime(2017, 10, 6, 0, 0, 0), card.Tarjeta.Caducidad.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporte, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(new DateTime(2016, 10, 7, 23, 59, 0), card.Titulo.FechaValidez1.Value);
			Assert.IsTrue(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(6, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(0, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(0, card.Titulo.ControlTarifa2.Value);
			// Recharges
			Assert.IsTrue(result.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[] {
					TitleCodeEnum.AbonoTransporte
				})
			);
			// Charges
			Assert.IsTrue(result.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[] {
					TitleCodeEnum.BonoTransbordo,
					TitleCodeEnum.AbonoTransporte,
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro
					//TitleCodeEnum.T1,
					//TitleCodeEnum.T2,
					//TitleCodeEnum.T3,
					//TitleCodeEnum.Sencillo,
					//TitleCodeEnum.IdaVuelta
				})
			);
		}
	}
}
