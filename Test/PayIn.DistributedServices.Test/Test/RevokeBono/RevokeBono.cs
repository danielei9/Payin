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
	public class RevokeBono : TransportBaseTest
	{
		[TestMethod]
		public async Task RevokeA()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			var charge = await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A, resultRead);

			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			
			var revoke = await RevokeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A, charge, charge.OperationId);

			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
		}
		[TestMethod]
		public async Task RevokeB()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			var charge = await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.B, resultRead);

			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);

			var revoke = await RevokeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.B, charge, charge.OperationId);

			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
		}
		[TestMethod]
		public async Task RevokeAB()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			var charge = await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A | EigeZonaEnum.B, resultRead);

			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);

			var revoke = await RevokeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A | EigeZonaEnum.B, charge, charge.OperationId);

			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
		}

		[TestMethod]
		public async Task RevokeABC()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			var charge = await ChargeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A | EigeZonaEnum.B | EigeZonaEnum.C, resultRead);

			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);

			var revoke = await RevokeCardAsync(card, TitleCodeEnum.Bonometro, EigeZonaEnum.A | EigeZonaEnum.B | EigeZonaEnum.C, charge, charge.OperationId);

			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(1003, card.Titulo.CodigoTitulo1.Value);

		}
	}
}
