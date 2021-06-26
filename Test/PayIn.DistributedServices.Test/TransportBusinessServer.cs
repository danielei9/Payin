using Newtonsoft.Json.Linq;
using PayIn.Domain.Transport.Eige.Types;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xp.Common;
using Xp.Domain.Transport;

namespace PayIn.DistributedServices.Test.Helpers
{
	public class TransportBusinessServer : BusinessServer
    {
		#region OperationGetReadInfoAsync
		public async Task<JObject> OperationGetReadInfoAsync(ITestCard card)
		{
			var result = await Server.GetAsync(
				"/Mobile/TransportOperation/v1/ReadInfo",
				new { MifareClassicCards = card.Uid.ToHexadecimal() }
			);
			return result;
		}
		#endregion OperationGetReadInfoAsync

		#region OperationReadInfoAsync
		public async Task<JObject> OperationReadInfoAsync(ITestCard card, int operationId, IEnumerable<MifareOperationResultArguments> lectura, XpDateTime now = null)
		{
			var result = await Server.PostAsync(
				"/Mobile/TransportOperation/v1/ReadInfo/" + operationId,
				new {
					cardId = card.Uid.ToHexadecimal(),
					script = lectura,
					now = now,
					isRead = true
				}
			);
			return result;
		}
		#endregion OperationReadInfoAsync

		#region TicketCreateAsync
		public async Task<JObject> TicketCreateAsync(JToken recharge, JToken price, XpDateTime now = null)
		{
			var lines = new List<object>
			{
				new {
					title = recharge.Value<string>("ownerName") + " - " + recharge.Value<string>("name") + " " + price.Value<string>("zoneName"),
					amount =  price.Value<decimal>("price"), //price.Value<decimal>("changePrice") != 0 ? price.Value<decimal>("price") +  price.Value<decimal>("changePrice") :
					quantity = 1
				}
			};
			if (price.Value<decimal>("changePrice") != 0)
			{
				lines.Add(new 
				{
					title = "Actualización títulos",
					amount = price.Value<decimal>("changePrice"),
					quantity = 1
				});
			}
			var result = await Server.PostAsync(
				"/Mobile/Ticket/v1",
				new
				{
					reference = "",
					date = now ?? DateTime.Now,
					concessionId = 1,
					lines = lines,
					liquidationConcession = 2
				}
			);
			return result;
		}
		#endregion TicketCreateAsync

		#region OperationRechargeAsync
		public async Task<JObject> OperationRechargeAsync(ITestCard card, int operationId, IEnumerable<MifareOperationResultArguments> lectura, JToken recharge, JToken price, int ticketId, XpDateTime now = null)
		{
			var result = await Server.PostAsync(
				"/Mobile/TransportOperation/v1/Recharge/" + operationId,
				new
				{
					cardType = (int)CardType.MIFAREClassic,
					cardId = card.Uid.ToHexadecimal(),
					script = lectura,
					code = recharge.Value<int>("code"),
					quantity = 1,
					priceId = price.Value<int>("id"),
					ticketId = ticketId,
					rechargeType = price.Value<int>("rechargeType"),
					imei = 123456789012345
					//device = 1,
					//promotion = 2
				}
			);
			return result;
		}
		#endregion OperationRechargeAsync

		#region OperationConfirmAndReadInfoAsync
		public async Task<JObject> OperationConfirmAndReadInfoAsync(ITestCard card, int operationId, IEnumerable<MifareOperationResultArguments> lectura, int slot, XpDateTime now = null)
		{
			var result = await Server.PutAsync(
				"/Mobile/TransportOperation/v1/ConfirmAndReadInfo/",
				operationId,
				new
				{
					cardId = card.Uid.ToHexadecimal(),
					script = lectura,
					slot = slot,
					now = now
				}
			);
			return result;
		}
		#endregion OperationConfirmAndReadInfoAsync

		#region OperationRevokeAsync
		public async Task<JObject> OperationRevokeAsync(ITestCard card, int operationId, IEnumerable<MifareOperationResultArguments> lectura, XpDateTime now = null)
		{
			var result = await Server.PostAsync(
				"/Mobile/TransportOperation/v1/Revoke",
				new
				{
					cardType = (int)CardType.MIFAREClassic,
					cardId = card.Uid.ToHexadecimal(),
					script = lectura,
					operationId = operationId,
					pin = 1234
				}
			);
			return result;
		}
		#endregion OperationRevokeAsync

		#region ModifyFieldAsync
		public async Task<JObject> ModifyFieldAsync(ITestCard card, string key, string value, long operationId)
		{
			var result = await Server.PostAsync(
				"/Api/GreyList/ModifyField",
				new
				{
					id = operationId,
					uid = Convert.ToInt64(card.Uid.ToInt32() ?? 0),
					modifyValues = new List<object>
					{
						new
						{
							Key = key,
							Value = value,
						}
					}
				}
			);
			return result;
		}
		#endregion ModifyFieldAsync
	}
}
