using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayIn.DistributedServices.Test.Cards;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.Test.RechargeAbono
{
	[TestClass]
	public class RechargeAbonoTCaducadoTest : TransportBaseTest
	{
		[TestMethod]
		public async Task Recharge()
		{
			//Título caducado sin recargas pendientes
			var card = new AbonoTCaducado();
			await Server.LoginAndroidAsync();

			var result = await ReadCardAsync(card);	
			Assert.IsTrue(result.Recharges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
					TitleCodeEnum.AbonoTransporte
				})
			);
			Assert.IsTrue(result.Charges
				.Select(x => (TitleCodeEnum)x.Value<int>("code"))
				.SetEquals(new[]
				{
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
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual((int)TitleCodeEnum.AbonoTransporte, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(EigeZonaEnum.A, card.Titulo.ValidezZonal1.Value);
			Assert.AreEqual(6, card.Titulo.ControlTarifa1.Value);

			await RechargeCardAsync(card, TitleCodeEnum.AbonoTransporte, EigeZonaEnum.A, result);
		}
	}
}
