using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PayIn.Infrastructure.Transport.Services.FgvService;

namespace PayIn.Application.Transport.Services
{
    public class FgvServerService : IServerService
    {
        [Dependency] public FgvService FgvService { get; set; }

        #region Recharged
        public async Task<IEnumerable<ServerResult>> Recharged(TransportOperation operation, Payment payment, string mobileSerial, DateTime now)
		{
            #region Old script
            var scriptOld = operation.ScriptPrevious.FromJson<JObject>();
			var scriptOldString = new string[64];
			foreach (var sector in scriptOld["Card"]["Sectors"])
			{
				var sectorNumber = sector.Value<int>("Number");
				foreach (var block in sector["Blocks"])
				{
					var blockNumber = block.Value<int>("Number");
					if (blockNumber != 3)
					{
						var value = block.Value<string>("Value");
						scriptOldString[sectorNumber * 4 + blockNumber] = value.FromBase64().ToHexadecimal();
					}
				}
            }
            #endregion Old script

            #region Old script
            var script = operation.ScriptPrevious.FromJson<JObject>();
			var scriptString = new string[64];
			foreach (var sector in script["Card"]["Sectors"])
			{
				var sectorNumber = sector.Value<int>("Number");
				foreach (var block in sector["Blocks"])
				{
					var blockNumber = block.Value<int>("Number");
					if (blockNumber != 3)
					{
						var value = block.Value<string>("Value");
						scriptString[sectorNumber * 4 + blockNumber] = value.FromBase64().ToHexadecimal();
					}
				}
            }
            #endregion Old script

            #region Get expendedor
            int? expendedor = null;
            if (operation.Ticket != null)
            {
                int temp = 0;
                if (int.TryParse(operation.Ticket.ExternalLogin, out temp))
                    expendedor = temp;
            }
            #endregion Get expendedor

            #region Registrar recarga
            var result = new ServerResult();
            try
            {
                result.Result = await FgvService.RedextOperacionAsync(
                    now,
                    operation.Uid,
                    operation.MobilisEnvironment,
                    operation.Price.Title.Code,
                    operation.Price.Zone,
                    operation.DateTimeValue,
                    (int)(
                        (operation.QuantityType == "€" ? 100 : 1) *
                        ((operation.QuantityValue ?? 0) - (operation.QuantityValueOld ?? 0))
                    ),
                    operation.MobilisAmpliationBit ?? false,
                    operation.Price.Version,
                    operation.OperationType,
                    operation.Slot,
                    operation.Ticket.Amount,
                    EigeFormaPagoEnum.Movil,
                    0,
                    true,
                    scriptOldString.JoinString("\n"),
                    scriptString.JoinString("\n"),
                    operation.Ticket.Payments.FirstOrDefault()?.AuthorizationCode ?? operation.Ticket.Reference,
                    operation.Id,
                    expendedor,
                    RedextOperacionType.Recharge
                );
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e.Message;
            }
            #endregion Registrar recarga

            return new ServerResult[] { result };
		}
		#endregion Recharged

		#region Revoked
		public async Task<IEnumerable<ServerResult>> Revoked(TransportOperation operation, string mobileSerial, DateTime now)
		{
            return await Task.Run(() =>
            {
                // De momento no hay revoke
                return new ServerResult[] { };
            });
        }
        #endregion Revoked

        #region Refund
        public async Task<IEnumerable<ServerResult>> Refund(TransportOperation operation, Payment payment, string mobileSerial, DateTime now)
        {
            #region Old script
            var scriptOld = operation.ScriptPrevious.FromJson<JObject>();
            var scriptOldString = new string[64];
            foreach (var sector in scriptOld["Card"]["Sectors"])
            {
                var sectorNumber = sector.Value<int>("Number");
                foreach (var block in sector["Blocks"])
                {
                    var blockNumber = block.Value<int>("Number");
                    if (blockNumber != 3)
                    {
                        var value = block.Value<string>("Value");
                        scriptOldString[sectorNumber * 4 + blockNumber] = value.FromBase64().ToHexadecimal();
                    }
                }
            }
            #endregion Old script

            #region Old script
            var script = operation.ScriptPrevious.FromJson<JObject>();
            var scriptString = new string[64];
            foreach (var sector in script["Card"]["Sectors"])
            {
                var sectorNumber = sector.Value<int>("Number");
                foreach (var block in sector["Blocks"])
                {
                    var blockNumber = block.Value<int>("Number");
                    if (blockNumber != 3)
                    {
                        var value = block.Value<string>("Value");
                        scriptString[sectorNumber * 4 + blockNumber] = value.FromBase64().ToHexadecimal();
                    }
                }
            }
            #endregion Old script

            #region Get expendedor
            int? expendedor = null;
            if (operation.Ticket != null)
            {
                int temp = 0;
                if (int.TryParse(operation.Ticket.ExternalLogin, out temp))
                    expendedor = temp;
            }
            #endregion Get expendedor

            #region Registrar recarga
            var rechargeResult = new ServerResult();
            try
            {
                rechargeResult.Result = await FgvService.RedextOperacionAsync(
                    now,
                    operation.Uid,
                    operation.MobilisEnvironment,
                    operation.Price.Title.Code,
                    operation.Price.Zone,
                    operation.DateTimeValue,
                    (int)(
                        (operation.QuantityType == "€" ? 100 : 1) *
                        ((operation.QuantityValue ?? 0) - (operation.QuantityValueOld ?? 0))
                    ),
                    operation.MobilisAmpliationBit ?? false,
                    operation.Price.Version,
                    operation.OperationType,
                    operation.Slot,
                    operation.Ticket.Amount,
                    EigeFormaPagoEnum.Movil,
                    0,
                    true,
                    scriptOldString.JoinString("\n"),
                    scriptString.JoinString("\n"),
                    operation.Ticket.Payments.FirstOrDefault()?.AuthorizationCode ?? operation.Ticket.Reference,
                    operation.Id,
                    expendedor,
                    RedextOperacionType.Recharge
                );
            }
            catch (Exception e)
            {
                rechargeResult.Success = false;
                rechargeResult.Exception = e.Message;
            }
            #endregion Registrar recarga

            #region Registrar devolución
            var refoundResult = new ServerResult();
            try
            {
                refoundResult.Result = await FgvService.RedextOperacionAsync(
                    now,
                    operation.Uid,
                    operation.MobilisEnvironment,
                    operation.Price.Title.Code,
                    operation.Price.Zone,
                    operation.DateTimeValue,
                    (int)(
                        (operation.QuantityType == "€" ? 100 : 1) *
                        ((operation.QuantityValue ?? 0) - (operation.QuantityValueOld ?? 0))
                    ),
                    operation.MobilisAmpliationBit ?? false,
                    operation.Price.Version,
                    operation.OperationType,
                    operation.Slot,
                    operation.Ticket.Amount,
                    EigeFormaPagoEnum.Movil,
                    0,
                    true,
                    scriptOldString.JoinString("\n"),
                    scriptString.JoinString("\n"),
                    "",
                    operation.Id,
                    expendedor,
                    RedextOperacionType.Refound
                );
            }
            catch (Exception e)
            {
                refoundResult.Success = false;
                refoundResult.Exception = e.Message;
            }
            #endregion Registrar devolución

            return new ServerResult[] { rechargeResult, refoundResult };
        }
        #endregion Refund

        #region WhiteListDownload
        public async Task<IEnumerable<WhiteListDto>> WhiteListDownload(DateTime now)
        {
            var listaWhiteAlicante = (await FgvService.RedextListaGrisAsync("A")).RESULTADO
                .Where(x => x.ACCION == 201) // 201 es una lista blanca
                .Select(x => new WhiteListDto
                {
                    Uid = x.NUM_SERIE,
                    Source = WhiteList.WhiteListSourceType.FgvAlacant,
                    OperationNumber = x.NUM_OPERACION,
                    State = WhiteList.WhiteListStateType.Active,
                    Amount = Convert.ToDecimal(x.VALOR) / 100m,
                    OperationType = WhiteListOperationType.Precharge,
                    TitleType = WhiteListTitleType.Title1,
                    ExclusionDate = null,
                    InclusionDate = null, // Se pondrá después al añadirlo a la BD como el UtcNow
                })
                .ToList();

            var listaWhiteValencia = (await FgvService.RedextListaGrisAsync("V")).RESULTADO
                .Where(x => x.ACCION == 201) // 201 es una lista blanca
                .Select(x => new WhiteListDto
                {
                    Uid = x.NUM_SERIE,
                    Source = WhiteList.WhiteListSourceType.FgvValencia,
                    OperationNumber = x.NUM_OPERACION,
                    State = WhiteList.WhiteListStateType.Active,
                    Amount = Convert.ToDecimal(x.VALOR) / 100m,
                    OperationType = WhiteListOperationType.Precharge,
                    TitleType = WhiteListTitleType.Title1,
                    ExclusionDate = null,
                    InclusionDate = null, // Se pondrá después al añadirlo a la BD como el UtcNow
                })
                .ToList();

            return listaWhiteAlicante.Union(listaWhiteValencia);
        }
        #endregion WhiteListDownload

        #region WhiteListMark
        public Task<string> WhiteListMark(WhiteList greyItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            throw new NotImplementedException();
        }
        #endregion WhiteListMark

        #region WhiteListUnmark
        public Task<string> WhiteListUnmark(WhiteList greyItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            throw new NotImplementedException();
        }
        #endregion WhiteListUnmark

        #region GreyListExecute
        public async Task<IEnumerable<GreyListDto>> GreyListDownload(DateTime now)
        {
            var listaGrisAlicante = (await FgvService.RedextListaGrisAsync("A")).RESULTADO
                .Where(x => x.ACCION != 201) // 201 es una lista blanca
                .Select(x => new GreyListDto
                {
                    Uid = x.NUM_SERIE,
                    OperationNumber = x.NUM_OPERACION,
                    Action = (GreyList.ActionType)x.ACCION,
                    Field = x.CAMPO,
                    NewValue = x.VALOR,
                    Resolved = (x.RESUELTO == 1),
                    Machine = GreyList.MachineType.All,
                    RegistrationDate = null, // Se pondrá después al añadirlo a la BD como el UtcNow
                    Source = GreyList.GreyListSourceType.FgvAlacant,
                    State = GreyList.GreyListStateType.Active
                })
                .ToList();

            var listaGrisValencia = (await FgvService.RedextListaGrisAsync("V")).RESULTADO
                .Where(x => x.ACCION != 201) // 201 es una lista blanca
                .Select(x => new GreyListDto
                {
                    Uid = x.NUM_SERIE,
                    OperationNumber = x.NUM_OPERACION,
                    Action = (GreyList.ActionType)x.ACCION,
                    Field = x.CAMPO,
                    NewValue = x.VALOR,
                    Resolved = (x.RESUELTO == 1),
                    Machine = GreyList.MachineType.All,
                    RegistrationDate = null, // Se pondrá después al añadirlo a la BD como el UtcNow
                    Source = GreyList.GreyListSourceType.FgvValencia,
                    State = GreyList.GreyListStateType.Active,
                })
                .ToList();

            return listaGrisAlicante.Union(listaGrisValencia);
        }
        #endregion GreyListGet

        #region GreyListMark
        public async Task<string> GreyListMark(GreyList greyItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                return "Grey list mark not implemented";
            });
        }
        #endregion GreyListMark

        #region GreyListUnmark
        public async Task<string> GreyListUnmark(TransportOperation operation, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                return "Grey list unmark not implemented";
            });
        }
        #endregion GreyListUnmark

        #region BlackListExecute
        public async Task<IEnumerable<BlackListDto>> BlackListDownload(DateTime now)
        {
            var listaNegraAlicante = (await FgvService.RedextListaNegraAsync("A")).RESULTADO
                .Select(x => new BlackListDto
                {
                    Uid = x.NUM_SERIE,
                    Rejection = (x.RECHAZO == 1),
                    Machine = BlackListMachineType.All,
                    RegistrationDate = null,
                    Resolved = false,
                    Source = BlackList.BlackListSourceType.FgvAlacant,
                    State = BlackList.BlackListStateType.Active
                })
                .GroupBy(x => x.Uid)
                .Select(x => x
                    .OrderBy(y => y.Rejection ? 0 : 1)
                    .FirstOrDefault()
                )
                .ToList();

            var listaNegraValencia = (await FgvService.RedextListaNegraAsync("V")).RESULTADO
                .Select(x => new BlackListDto
                {
                    Uid = x.NUM_SERIE,
                    Rejection = (x.RECHAZO == 1),
                    Machine = BlackListMachineType.All,
                    RegistrationDate = null,
                    Resolved = false,
                    Source = BlackList.BlackListSourceType.FgvValencia,
                    State = BlackList.BlackListStateType.Active
                })
                .GroupBy(x => x.Uid)
                .Select(x => x
                    .OrderBy(y => y.Rejection ? 0 : 1)
                    .FirstOrDefault()
                )
                .ToList();

            return listaNegraAlicante.Union(listaNegraValencia);

        }
        #endregion BlackListExecute

        #region BlackListMark
        public async Task<string> BlackListMark(BlackList blackList, TransportOperation operation, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                return "Black list mark not implemented";
            });
            //var listaNegraValencia = await FgvService.RedextEjecutarListaNegraAsync(
            //    //operation.Id,
            //    //now,
            //    "v"
            //);

            ////var result = listaNegraValencia.RESULTADO
            ////	.Select(x => new BlackList
            ////	{
            ////		Id = x.ID,
            ////		Uid = x.NUM_SERIE,
            ////		Rejection = x.RECHAZO
            ////	});
            //return "Ok";
        }
        #endregion BlackListMark

        #region BlackListUnmark
        public async Task<string> BlackListUnmark(TransportOperation operation, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                return "Black list unmark not implemented";
            });
        }
        #endregion BlackListUnmark
    }
}
