using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test.Replace
{
	[TestClass]
	public class ReplaceBT : TransportBaseTest
	{
		/// <summary>
		/// Sí se puede pasar de un BT a un AT ya que se pasa de un bono10 a un temporal
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Read()
		{
			var card = new BonoTransbordoCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			// Titulo 1
			Assert.AreEqual((int)TitleCodeEnum.BonoTransbordo, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(8, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
		}
		/// <summary>
		/// Açò no es pot recarregar perque el soport no estar personalitzat
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task RechargeAT_SupportNoPerso()
		{
			var now = new DateTime(2016, 10, 11, 10, 0, 0);
			var card = new BonoTransbordoCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card, now);
			Assert.IsTrue(resultRead.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.BonoTransbordo
				})
			);
			Assert.IsTrue(resultRead.Charges
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

			try
			{
				await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporte, EigeZonaEnum.A, resultRead, now);
			}
			catch (AssertFailedException ex)
			{
				Assert.AreEqual("Assert.IsNotNull failed. No se ha encontrado el titulo " + ((int)TitleCodeEnum.AbonoTransporte), ex.Message);
				return;
			}
			Assert.Fail("Exception of type {0} should be thrown.", typeof(AssertFailedException));
		}
		/// <summary>
		/// No se puede pasar de BT a T1 ya que T1 es de solo un dia
		/// </summary>
		/// <returns></returns>
		
		public async Task RechargeT1()
		{
			var card = new BonoTransbordoCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			await ChargeCardAsync(card, TitleCodeEnum.T1, null, resultRead);
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.T1, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			Assert.AreEqual(24, card.Titulo.NumeroUnidadesValidezTemporal1.Value);
			Assert.AreEqual(EigeTipoUnidadesValidezTemporalEnum.Horas, card.Titulo.TipoUnidadesValidezTemporal1.Value);
		}
		/// <summary>
		/// No se puede pasar de BT a T2 ya que T2 es de solo dos dia
		/// </summary>
		/// <returns></returns>		
		public async Task RechargeT2()
		{
			var card = new BonoTransbordoCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			
			await ChargeCardAsync(card, TitleCodeEnum.T2, null, resultRead);
			Assert.AreEqual((int)TitleCodeEnum.T2, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			Assert.AreEqual(48, card.Titulo.NumeroUnidadesValidezTemporal1.Value);
			Assert.AreEqual(EigeTipoUnidadesValidezTemporalEnum.Horas, card.Titulo.TipoUnidadesValidezTemporal1.Value);
		}
		/// <summary>
		/// No se puede pasar de BT a T3 ya que T3 es de solo tres dias
		/// </summary>
		/// <returns></returns>		
		public async Task RechargeT3()
		{
			var card = new BonoTransbordoCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);

			await ChargeCardAsync(card, TitleCodeEnum.T3, null, resultRead);
			Assert.AreEqual((int)TitleCodeEnum.T3, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(5, card.Titulo.ControlTarifa1.Value);
			Assert.AreEqual(72, card.Titulo.NumeroUnidadesValidezTemporal1.Value);
			Assert.AreEqual(EigeTipoUnidadesValidezTemporalEnum.Horas, card.Titulo.TipoUnidadesValidezTemporal1.Value);
		}
	}
}
