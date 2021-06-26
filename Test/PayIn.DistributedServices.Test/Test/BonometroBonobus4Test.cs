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
	public class BonometroBonobus4Test : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var card = new BonometroBonobus4Card();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);

			var recharges = result.Recharges;
			Assert.IsTrue(recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro
				}))
			;
			var charges = result.Charges;
			Assert.IsTrue(charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.Bonobus,
					TitleCodeEnum.Bonometro
				})
			);

			Assert.AreEqual(card.Titulo.TitulosActivos.Value, EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2);
			// Titulo 1
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(7, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(96, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(19, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);
		}
		[TestMethod]
		public async Task RechargeBonobus()
		{
			var card = new BonometroBonobus4Card();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			Assert.AreEqual(19, card.Titulo.SaldoViaje2.Value);


			var charge = await ChargeCardAsync(card, 96, null, resultRead);
			
			Assert.AreEqual(card.Titulo.TitulosActivos.Value, EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2);
			// Titulo 1
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(7, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(96, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(29, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);
		}
		[TestMethod]
		public async Task RevokeBonobus()
		{
			var card = new BonometroBonobus4Card();
			await Server.LoginAndroidAsync();
			var now = DateTime.Now.ToUTC();

			var resultRead = await ReadCardAsync(card);
			var charge = await ChargeCardAsync(card, 96, EigeZonaEnum.A, resultRead,now);
			
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);
			Assert.AreEqual(7, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
			// Titulo 2
			Assert.AreEqual(96, card.Titulo.CodigoTitulo2.Value);
			Assert.AreEqual(29, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);

			var revoke = await RevokeCardAsync(card, 96, EigeZonaEnum.A, charge, charge.OperationId,now);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1 | EigeTitulosActivosEnum.Titulo2, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(96, card.Titulo.CodigoTitulo2.Value);
			Assert.IsNull(card.Titulo.FechaValidez2.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion2.Value);
			Assert.AreEqual(19, card.Titulo.SaldoViaje2.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa2.Value);
		}
	}
}
