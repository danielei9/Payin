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
	public class FranTest : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var card = new FrankCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(9, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
		}
		[TestMethod]
		public async Task RechargeRechargeRevokeErrorLog()
		{
			var now = new DateTime(2016, 10, 15, 10, 00, 0);

			var card = new Frank2Card();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(18, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);

			var resultCharge = await ChargeCardAsync(card, TitleCodeEnum.Bonometro | TitleCodeEnum.Bonobus, EigeZonaEnum.A, resultRead);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(28, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);

			var resultCharge2 = await ChargeCardAsync(card, TitleCodeEnum.Bonobus, EigeZonaEnum.A, resultCharge);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(EigePosicionUltimaCargaEnum.Carga1, card.Carga.PosicionUltima.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(28, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);
			// Check 1
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Carga.CodigoTitulo1.Value);
			Assert.AreEqual(EigeTipoOperacionCarga_OpcionEnum.Titulo1, card.Carga.TipoOperacion1_Opcion.Value);
			Assert.AreEqual(EigeTipoOperacionCarga_OperacionEnum.Recarga, card.Carga.TipoOperacion1_Operacion.Value);
			// Check 2
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Carga.CodigoTitulo2.Value);
			Assert.AreEqual(EigeTipoOperacionCarga_OpcionEnum.Titulo2, card.Carga.TipoOperacion2_Opcion.Value);
			Assert.AreEqual(EigeTipoOperacionCarga_OperacionEnum.Recarga, card.Carga.TipoOperacion2_Operacion.Value);

			await RevokeCardAsync(card, TitleCodeEnum.Bonobus, EigeZonaEnum.A, resultCharge2, resultCharge2.OperationId);
			Assert.AreEqual(EigePosicionUltimaCargaEnum.Carga2, card.Carga.PosicionUltima.Value);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual((int)TitleCodeEnum.Bonometro, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(28, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);
			// Check 1
			Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Carga.CodigoTitulo1.Value);
			Assert.AreEqual(EigeTipoOperacionCarga_OpcionEnum.Titulo1, card.Carga.TipoOperacion1_Opcion.Value);
			Assert.AreEqual(EigeTipoOperacionCarga_OperacionEnum.Recarga, card.Carga.TipoOperacion1_Operacion.Value);
			// Check 2
		    Assert.AreEqual((int)TitleCodeEnum.Bonobus, card.Carga.CodigoTitulo2.Value);
			Assert.AreEqual(EigeTipoOperacionCarga_OpcionEnum.Titulo1, card.Carga.TipoOperacion2_Opcion.Value);
			Assert.AreEqual(EigeTipoOperacionCarga_OperacionEnum.Anulacion, card.Carga.TipoOperacion2_Operacion.Value);
		}
	}
}
