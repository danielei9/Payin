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
	public class Marcela3Test : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var now = new DateTime(2016, 10, 25);
			var card = new Marcela3Card();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card, now);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(7, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(9, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);
			// Recharges
			Assert.IsTrue(result.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[] {
					TitleCodeEnum.Bonometro,
					TitleCodeEnum.Bonobus
				})
			);
			// Charges
			Assert.IsTrue(result.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonometro,
					TitleCodeEnum.Bonobus
				})
			);
		}
	}
}
