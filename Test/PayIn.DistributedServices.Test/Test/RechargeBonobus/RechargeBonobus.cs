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
	public class RechargeBonobus : TransportBaseTest
	{
		[TestMethod]
		public async Task Recharge()
		{
			var card = new BonobusBonometroCard();
			await Server.LoginAndroidAsync();

			var resultRead = await ReadCardAsync(card);
			var charge = await ChargeCardAsync(card, 96, null, resultRead);

			try
			{
				await ChargeCardAsync(card, TitleCodeEnum.AbonoTransporte, null, resultRead);
			}
			catch (AssertFailedException ex)
			{
				Assert.AreEqual("Assert.IsNotNull failed. No se ha encontrado el titulo " + ((int)TitleCodeEnum.AbonoTransporte), ex.Message);
				return;
			}
			Assert.Fail("Exception of type {0} should be thrown.", typeof(AssertFailedException));
		}
	}
}
