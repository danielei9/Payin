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
	public class RevokeBonobus : TransportBaseTest
	{
		[TestMethod]
		public async Task Revoke()
		{
			var card = new EmptyCard();
			await Server.LoginAndroidAsync();

			// Leer
			var resultRead = await ReadCardAsync(card);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			// Recarga
			var charge = await ChargeCardAsync(card, TitleCodeEnum.Bonobus, EigeZonaEnum.A, resultRead);
			Assert.AreEqual(10, card.Titulo.SaldoViaje1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);

			// Devolución
			await RevokeCardAsync(card, TitleCodeEnum.Bonobus, EigeZonaEnum.A, charge, charge.OperationId);

			
			Assert.AreEqual(EigeTitulosActivosEnum.Titulo1, card.Titulo.TitulosActivos.Value);
			Assert.AreEqual(96, card.Titulo.CodigoTitulo1.Value);
			Assert.IsNull(card.Titulo.FechaValidez1.Value);
			Assert.IsFalse(card.Titulo.TituloEnAmpliacion1.Value);
			Assert.AreEqual(0, card.Titulo.SaldoViaje1.Value);
			Assert.AreEqual(4, card.Titulo.ControlTarifa1.Value);
		}
		
	}
}
