using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test
{
	[TestClass]
	public class BonoTransbordoTest : TransportBaseTest
	{
		[TestMethod]
		public async Task Read()
		{
			var card = new BonoTransbordoCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.IsTrue(result.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.BonoTransbordo
				}));
			Assert.IsTrue(result.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
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
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
		}
		[TestMethod]
		public async Task Recharge()
		{
			var card = new BonoTransbordoCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result);
			Assert.AreEqual(card.Titulo.TitulosActivos.Value, EigeTitulosActivosEnum.Titulo1);
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(18, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
		}

		//[TestMethod]
		//public async Task RechargeObsolete()
		//{
		//	var card = new BonoTransbordoObsoletoCard();
		//	await Server.LoginAndroidAsync();

		//	var result = await ReadCardAsync(card);
		//	Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
		//	Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);

		//	await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result);		
		//}

		[TestMethod]
		public async Task Recharge19V()
		{
			var card = new BonoTransbordo19VCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(19, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			var result2 = await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(29, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			await RevokeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result2, result2.OperationId);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(19, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
		}
		[TestMethod]
		public async Task Revoke()
		{
			var card = new BonoTransbordoCard();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			result = await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			//Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(18, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			await RevokeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result, result.OperationId);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
		}
		[TestMethod]
		public async Task GreyList()
		{
			var card = new BonoTransbordoCard();

			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			await Server.LoginWebAsync();
			var res = await Server.ModifyFieldAsync(card, "TV1SV", "5", result.OperationId);
			Assert.AreEqual(1, res.Value<int>("count"));

			await Server.LoginAndroidAsync();

			result = await ReadCardAsync(card);
			var writes = result.Card
				.Where(x => x.Operation == Xp.Domain.Transport.MifareClassic.MifareOperationType.WriteBlock)
				.ToList();
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(5, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
		}
		[TestMethod]
		public async Task GreyList_Recharge()
		{
			var card = new BonoTransbordoCard();

			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			await Server.LoginWebAsync();
			var res = await Server.ModifyFieldAsync(card, "TV1SV", "5", result.OperationId);
			Assert.AreEqual(1, res.Value<int>("count"));

			await Server.LoginAndroidAsync();

			result = await ReadCardAsync(card);
			var writes = result.Card
				.Where(x => x.Operation == Xp.Domain.Transport.MifareClassic.MifareOperationType.WriteBlock)
				.ToList();
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(5, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			result = await ChargeCardAsync(card, TitleCodeEnum.BonoTransbordo, EigeZonaEnum.A, result);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(15, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);

			result = await ReadCardAsync(card);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(15, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
		}
	}
}
