using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PayIn.DistributedServices.Test.Helpers;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Common;

namespace PayIn.DistributedServices.Test
{
	public class TransportBaseTest : BaseTest<TransportBusinessServer>
    {
        #region ReadCardAsync
        public async Task<ResultRead> ReadCardAsync(TestCard card, XpDateTime now = null)
		{
			// GetReadInfo
			var getReadInfo = await Server.OperationGetReadInfoAsync(card);
			var operationId = getReadInfo["operation"].Value<int>("id");
			Assert.IsNotNull(operationId);
			var getReadInfoResult = card.Execute(getReadInfo["scripts"].FirstOrDefault());
			Assert.IsNotNull(getReadInfoResult);

			// ReadInfo
			var readInfo = await Server.OperationReadInfoAsync(card, operationId, getReadInfoResult, now);
			var readInfoResult = card.Execute(readInfo["scripts"].FirstOrDefault());
			Assert.IsNotNull(readInfoResult);
			if (
				card.Sectors[1].Blocks[1].Value.Length == 16 &&
				card.Sectors[1].Blocks[2].Value.Length == 16
			)
			Assert.IsTrue(card.Sectors[1].Blocks[1].Value.SequenceEqual(card.Sectors[1].Blocks[2].Value));
			if (
				card.Sectors[4].Blocks[0].Value.Length == 16 &&
				card.Sectors[4].Blocks[1].Value.Length == 16
			)
				Assert.IsTrue(card.Sectors[4].Blocks[0].Value.SequenceEqual(card.Sectors[4].Blocks[1].Value));

			// ConfirmAndRead
			var confirmed = await Server.OperationConfirmAndReadInfoAsync(card, operationId, readInfoResult, 0, now);
			var confirmedResult = card.Execute(confirmed["scripts"].FirstOrDefault());

			// Check Crc
			foreach (var sector in card.Sectors)
				foreach (var block in sector.Blocks)
					Assert.IsTrue(await card.CheckAsync(card.Uid.ToInt32().Value, block), "Bloque con CRC error {0}-{1}".FormatString(sector.Number, block.Number));

			return new ResultRead
			{
				OperationId = operationId,
				Card = confirmedResult,
				Charges = readInfo["chargeTitles"],
				Recharges = readInfo["rechargeTitles"],
				Fields = readInfo
			};
		}
		#endregion ReadCardAsync

		#region ChargeCardAsync
		public async Task<ResultRead> ChargeCardAsync(TestCard card, TitleCodeEnum titulo, EigeZonaEnum? zone, ResultRead resultRead, XpDateTime now = null)
		{
			return await ChargeCardAsync(card, (int)titulo, zone, resultRead, now);
		}
		public async Task<ResultRead> ChargeCardAsync(TestCard card, int code, EigeZonaEnum? zone, ResultRead resultRead, XpDateTime now = null)
		{
			var isRechargable = resultRead.Fields["isRechargable"];
			Assert.IsTrue(isRechargable != null, "No se puede recargar esta tarjeta");
			Assert.IsTrue(isRechargable.Value<bool>(), "No se puede recargar esta tarjeta");

			var charge = resultRead.Charges.Where(x => x.Value<int>("code") == code).FirstOrDefault();
			Assert.IsNotNull(charge, "No se ha encontrado el titulo {0}".FormatString(code));

			var price = zone == null ?
			   charge["prices"]
				   .FirstOrDefault() :
			   charge["prices"]
				   .Where(x => x.Value<int>("zone") == (int)zone)
				   .FirstOrDefault();

			if (IsTuiN(code))
				price["price"] = 10;

			Assert.IsNotNull(price, "No se ha encontrado el precio de la zona {0}".FormatString(zone));

			// Create ticket
			var ticketCreate = await Server.TicketCreateAsync(charge, price, now);
			var ticketId = ticketCreate.Value<int>("id");
			Assert.IsNotNull(ticketId);

			// Charge
			var charged = await Server.OperationRechargeAsync(card, resultRead.OperationId, resultRead.Card, charge, price, ticketId, now);
			var operationId = charged["operation"].Value<int>("id");
			Assert.IsNotNull(operationId);
			var rechargedResult = card.Execute(charged["scripts"].FirstOrDefault());

			// ConfirmAndRead
			var confirmed = await Server.OperationConfirmAndReadInfoAsync(card, operationId, rechargedResult, price.Value<int>("slot"), now);

			// Check Crc
			foreach (var sector in card.Sectors)
				foreach (var block in sector.Blocks)
					Assert.IsTrue(await card.CheckAsync(card.Uid.ToInt32().Value, block), "Bloque con CRC error {0}-{1}".FormatString(sector.Number, block.Number));

			return new ResultRead
			{
				OperationId = operationId,
				Card = rechargedResult,
				Charges = confirmed["chargeTitles"],
				Recharges = confirmed["rechargeTitles"],
				Fields = confirmed
			};
		}
		#endregion ChargeCardAsync

		#region RechargeCardAsync
		public async Task<ResultRead> RechargeCardAsync(TestCard card, TitleCodeEnum titulo, EigeZonaEnum? zone, ResultRead resultRead, XpDateTime now = null)
		{
			return await RechargeCardAsync(card, (int)titulo, zone, resultRead, now);
		}
		public async Task<ResultRead> RechargeCardAsync(TestCard card, int code, EigeZonaEnum? zone, ResultRead resultRead, XpDateTime now = null)
		{
			var isRechargable = resultRead.Fields["isRechargable"];
			Assert.IsTrue(isRechargable != null, "No se puede recargar esta tarjeta");
			Assert.IsTrue(isRechargable.Value<bool>(), "No se puede recargar esta tarjeta");

			var recharge = resultRead.Recharges.Where(x => x.Value<int>("code") == code).FirstOrDefault();
			Assert.IsNotNull(recharge, "No se ha encontrado el titulo {0}".FormatString(code));

			var price = zone == null ?
				recharge["prices"]
					.FirstOrDefault() :
				recharge["prices"]
					.Where(x => x.Value<int>("zone") == (int)zone)
					.FirstOrDefault();
			Assert.IsNotNull(price, "No se ha encontrado el precio de la zona {0}".FormatString(zone));

			// Create ticket
			var ticketCreate = await Server.TicketCreateAsync(recharge, price, now);
			var ticketId = ticketCreate.Value<int>("id");
			Assert.IsNotNull(ticketId);

			// Charge
			var recharged = await Server.OperationRechargeAsync(card, resultRead.OperationId, resultRead.Card, recharge, price, ticketId, now);
			var operationId = recharged["operation"].Value<int>("id");
			Assert.IsNotNull(operationId);
			var rechargedResult = card.Execute(recharged["scripts"].FirstOrDefault());

			// ConfirmAndRead
			var confirmed = await Server.OperationConfirmAndReadInfoAsync(card, operationId, rechargedResult, price.Value<int>("slot"), now);

			// Check Crc
			foreach (var sector in card.Sectors)
				foreach (var block in sector.Blocks)
					Assert.IsTrue(await card.CheckAsync(card.Uid.ToInt32().Value, block), "Bloque con CRC error {0}-{1}".FormatString(sector.Number, block.Number));

			return new ResultRead
			{
				OperationId = operationId,
				Card = rechargedResult,
				Charges = confirmed["chargeTitles"],
				Recharges = confirmed["rechargeTitles"],
				Fields = confirmed
			};
		}
		#endregion RechargeCardAsync

		#region IsTuiN
		public bool IsTuiN(int? code)
		{
			return ((code >= 1271) && (code <= 1277));
		}
		#endregion IsTuiN

		#region RevokeCardAsync
		public async Task<ResultRead> RevokeCardAsync(TestCard card, TitleCodeEnum code, EigeZonaEnum? zone, ResultRead resultRead, int operationId, XpDateTime now = null)
		{
			return await RevokeCardAsync(card, (int)code, zone, resultRead, operationId, now);
		}
		public async Task<ResultRead> RevokeCardAsync(TestCard card, int code, EigeZonaEnum? zone, ResultRead resultRead, int operationId, XpDateTime now = null)
		{
			var isRevokable = (bool)resultRead.Fields["isRevokable"];
			if (!isRevokable)
				throw new ApplicationException("Esta recarga no se puede devolver");
			var revokablePrice = (int)resultRead.Fields["revokablePrice"];

			// Revoke
			var revoked = await Server.OperationRevokeAsync(card, operationId, resultRead.Card, now);
			var revokedResult = card.Execute(revoked["scripts"].FirstOrDefault());
			var revokedOperationId = revoked["operation"].Value<int>("id");

			// ConfirmAndRead
			var confirmedResult = await Server.OperationConfirmAndReadInfoAsync(card, revokedOperationId, revokedResult, revokablePrice, now);

			// Check Crc
			foreach (var sector in card.Sectors)
				foreach (var block in sector.Blocks)
					Assert.IsTrue(await card.CheckAsync(card.Uid.ToInt32().Value, block), "Bloque con CRC error {0}-{1}".FormatString(sector.Number, block.Number));

			return new ResultRead
			{
				OperationId = operationId,
				Card = revokedResult,
				Charges = null, //confirmed["chargeTitles"],
				Recharges = null //confirmed["rechargeTitles"],
				//Fields = confirmedResult
			};
		}
		#endregion RevokeCardAsync
	}
}
