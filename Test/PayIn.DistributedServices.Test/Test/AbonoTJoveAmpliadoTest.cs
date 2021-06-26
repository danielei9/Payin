using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Common;

namespace PayIn.DistributedServices.Test
{
	[TestClass]
	public class AbonoTJoveAmpliadoTest : TransportBaseTest
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
					TitleCodeEnum.Bonometro,
					TitleCodeEnum.Bonobus
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
		/// <summary>
		/// Recarga de un abono agotado y ampliado
		/// </summary>
		/// <returns></returns>
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
		public async Task RechargeExpiredAmpliated()
		{
			//Debe dejar recargar porque el título ya ha caducado
			var now = new DateTime(2016, 7, 20, 8, 57, 0);

			var card = new AbonoTJoveAmpliadoCard();
			await Server.LoginAndroidAsync();
			var resultRead = await ReadCardAsync(card);			
		
			await RechargeCardAsync(card, TitleCodeEnum.AbonoTransporteJove, EigeZonaEnum.A, resultRead, now);	

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporteJove, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsTrue(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(6, card.Titulo.ControlTarifa1.Value);
			Assert.IsTrue(card.Titulo.TituloEnAmpliacion1.Value);
		}
	}
}
