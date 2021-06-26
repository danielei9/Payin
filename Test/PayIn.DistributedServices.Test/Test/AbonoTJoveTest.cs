using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test
{
	[TestClass]
	public class AbonoTJoveTest : TransportBaseTest
	{
		/// <summary>
		/// Se va a recargar en periodo de jove
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task RechargeInJoveTime()
		{
			var now = new DateTime(2016, 1, 1);

			var card = new AbonoTJoveCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card, now);
			Assert.AreEqual(new DateTime(2016, 5, 23), card.Tarjeta.Caducidad.Value);
			Assert.AreEqual(new DateTime(2011, 10, 23), card.Emision.FechaEmision.Value);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporteJove, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);

			Assert.IsTrue(result.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[] {
					TitleCodeEnum.AbonoTransporteJove
				})
			);
			Assert.IsTrue(result.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[] {
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
		}
		/// <summary>
		/// Se va a recargar cuando ha terminado el periodo de jove
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task RechargeNotInJoveTime()
		{
			var now = new DateTime(2016, 8, 1);

			var card = new AbonoTJoveCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card, now);
			Assert.AreEqual(new DateTime(2016, 5, 23), card.Tarjeta.Caducidad.Value);
			Assert.AreEqual(new DateTime(2011, 10, 23), card.Emision.FechaEmision.Value);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporteJove, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);

			Assert.IsTrue(result.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new TitleCodeEnum[0])
			);
			Assert.IsTrue(result.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[] {
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
		}
		/// <summary>
		/// Se va a recargar cuando ha terminado el periodo de jove y está caducado
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task RechargeNotInTime()
		{
			var now = new DateTime(2016, 11, 1);

			var card = new AbonoTJoveCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card, now);
			Assert.AreEqual(new DateTime(2016, 5, 23), card.Tarjeta.Caducidad.Value);
			Assert.AreEqual(new DateTime(2011, 10, 23), card.Emision.FechaEmision.Value);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporteJove, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);

			Assert.IsTrue(result.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new TitleCodeEnum[0])
			);
			Assert.IsTrue(result.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new TitleCodeEnum[0])
			);
		}
	}
}
