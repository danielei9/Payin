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
	public class EmptyTest : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();
			
			var result = await ReadCardAsync(card);

			var recharges = result.Recharges;
			Assert.AreEqual(0, recharges.Count());
			var charges = result.Charges;
			Assert.IsTrue(charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro,
					TitleCodeEnum.BonoTransbordo,
					TitleCodeEnum.AbonoTransporte,
					TitleCodeEnum.AbonoTransporteJove
					//TitleCodeEnum.T1,
					//TitleCodeEnum.Sencillo,
					//TitleCodeEnum.IdaVuelta,
					//TitleCodeEnum.T2,
					//TitleCodeEnum.T3
				})
			);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
		}
		[TestMethod]
		public async Task ChargeBonometroA()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);

			var charge = await ChargeCardAsync(card, 1003, EigeZonaEnum.A, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
		}
		/// <summary>
		/// Tras el canje de un Bonometro zona A a B no debe dejar la devolución
		/// Recomendación: Especificar los distintos errores!
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task ExchangeBonometroAtoBNoRevokable()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);

			result = await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A, result);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);

			result = await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A | EigeZonaEnum.B, result);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(EigeZonaEnum.A | EigeZonaEnum.B, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(20, card.Titulo.SaldoViaje1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(null, card.Titulo.FechaValidez1.Value);
			
			try
			{
				await RevokeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A | EigeZonaEnum.B, result, result.OperationId);
			}
			catch (ApplicationException ex)
			{
				Assert.AreEqual("Esta recarga no se puede devolver", ex.Message);
				return;
			}
			Assert.Fail("Exception of type {0} should be thrown.", typeof(ApplicationException));
		}
		[TestMethod]
		public async Task ChargeBonoTransbordoA()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);

			var charge = await ChargeCardAsync(card, 1552, EigeZonaEnum.A, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(1552, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
		}

		[TestMethod]
		public async Task ChargeTuiN()
		{
			var card = new EmptyFGVCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);

			var charge = await ChargeCardAsync(card, TitleCodeEnum.TuiN, EigeZonaEnum.A, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.TuiN, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
		}
		[TestMethod]
		public async Task ChargeAbonoTransporteA()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);

			var charge = await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporte, EigeZonaEnum.A, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporte, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
		}
		[TestMethod]
		public async Task ChargeAbono2TransporteA()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);

			var charge = await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporte, EigeZonaEnum.A, resultRead);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporte, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);

			var charge2 = await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporte, EigeZonaEnum.A, charge);
			// Cuando pongamos el confirm and readinfo se ha de pasar esto a la funcion y no el resultRead

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporte, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsTrue(card.Titulo.TituloEnAmpliacion1.Value);
		}
		[TestMethod]
		public async Task RevokeBonobusFromEmptyCard()
		{
			//La devolucion a tarjeta vacía deberá dejar los datos del último título recargado sin viajes
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);

			var charge = await ChargeCardAsync(card, 96, EigeZonaEnum.A, resultRead);

			var now = DateTime.Now;
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual(96, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			//Para bonobús verificar si la fecha ha de ser la actual
			//Assert.AreEqual(new DateTime(2000, 1, 1), card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			// Titulo 2
			Assert.AreEqual(0, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(0, card.Titulo.ControlTarifa2.Value);

			var revoke = await RevokeCardAsync(card, 96, EigeZonaEnum.A, charge, charge.OperationId);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(96, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
		}

		private class GenericEnum<T>
		{
			private byte[] v1;
			private int v2;

			public GenericEnum(byte[] v1, int v2)
			{
				this.v1 = v1;
				this.v2 = v2;
			}
		}
	}
}
