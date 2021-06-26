using Microsoft.Practices.Unity;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Promotions;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Domain;
using Xp.Domain.Transport;

namespace PayIn.Application.Transport.Services
{
    public class EmtServerService : IServerService
	{
		[Dependency] public IEntityRepository<TransportOperation> Repository { get; set; }
		[Dependency] public IEntityRepository<PromoExecution> PromoExecutionRepository { get; set; }
		[Dependency] public EmtService EmtService { get; set; }

		#region Recharged
		public async Task<IEnumerable<ServerResult>> Recharged(TransportOperation operation, Payment payment, string mobileSerial, DateTime now)
        {
            var result = new ServerResult();
            try
            {
                var lastRecharge = new TransportOperation();
                if (operation != null && operation.OperationType == OperationType.Revoke)
                    lastRecharge = (await Repository.GetAsync())
                        .Where(x => x.Uid == operation.Uid && x.OperationDate < operation.OperationDate && x.OperationType == OperationType.Recharge)
                        .OrderByDescending(x => x.OperationDate)
                        .FirstOrDefault();

                //Obtención del número de viajes de la promoción aplicada
                var execution = new PromoExecution();
                var sumQuantity = 0;
                if (operation.PromoExecutionId != null)
                {
                    execution = (await PromoExecutionRepository.GetAsync("Promotion.PromoActions"))
                        .Where(x => x.Id == operation.PromoExecutionId && x.AppliedDate != null)
                        .FirstOrDefault();

                    sumQuantity = execution.Promotion.PromoActions
                        .Where(x => x.Type == PromoActionType.MoreTravel)
                        .Sum(x => (int?)x.Quantity) ?? 0;
                }

                var payIn = "PY";
                var script = GetScriptSearch(operation.Script);
                var operationscript = operation.Script.FromJson();
                var device = operation.Device.FromJson();
                var uid = operation.Uid == null ? 0 : (long)operation.Uid;
                var operationId = payIn + operation.Id;
                var EMTDate = EmtService.FormatDateEMT((DateTime)operation.ConfirmationDate);
                var ticket = operation.TicketId;
                var operationCode = EmtService.OperationCodeEMT(operation);
                var amount = (operation.OperationType == OperationType.Revoke) ? Convert.ToInt32(operation.Price.Price * -100) : Convert.ToInt32(operation.Price.Price * 100);
                var cellularNumber = device == null ? "" : device.suscriberId;
                var saldoViajes = EmtService.GetSaldoViajes(script);
                var validity = EmtService.GetValidity(script);
                var ampliated = EmtService.GetAmpliated(script);
                var version = operation.Price.Version;
                var cod_viajero = EmtService.GetCodViajero(script);
                var cod_anulacion = (lastRecharge.Id == 0) ? "" : payIn + lastRecharge.Id.ToString();
                var cardType = script.Card.Tarjeta.Tipo.Value.ToString();
                var cardSubType = Convert.ToString((int)script.Card.Tarjeta.Tipo.Value) + Convert.ToString((int)script.Card.Tarjeta.Subtipo.Value);

#if TEST || HOMO || DEBUG || EMULATOR
			    var operationType = "S";
#else
                var operationType = "R";
#endif

                string entry = "18," + operationId + "," + EMTDate + "," + ticket + "," + uid + "," + operationCode + "," + amount + ",0,96,A," + cellularNumber + ",0," + saldoViajes + "," + validity + "," + ampliated + "," + version + "," + cod_viajero + "," + cod_anulacion + "," + "11" + ",1," + operationType + "," + payIn + "," + sumQuantity + Environment.NewLine;

                //Revisar el tipo de script
                result.Result = (await EmtService.PostEMTFileOperationsAsync(entry))
                    .ToString();
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e.Message;
            }

			return new ServerResult[] { result };
		}
		#endregion Recharged

		#region Revoked
		public async Task<IEnumerable<ServerResult>> Revoked(TransportOperation operation, string mobileSerial, DateTime now)
        {
            var result = new ServerResult();
            try
            {
                var lastRecharge = new TransportOperation();
			    if (operation != null && operation.OperationType == OperationType.Revoke)
				    lastRecharge = (await Repository.GetAsync())
					    .Where(x => x.Uid == operation.Uid && x.OperationDate < operation.OperationDate && x.OperationType == OperationType.Recharge)
					    .OrderByDescending(x => x.OperationDate)
					    .FirstOrDefault();

			    //Obtención del número de viajes de la promoción aplicada
			    var execution = new PromoExecution();
			    var sumQuantity = 0;
			    if (operation.PromoExecutionId != null)
			    {
				    execution = (await PromoExecutionRepository.GetAsync("Promotion.PromoActions"))
					    .Where(x => x.Id == operation.PromoExecutionId && x.AppliedDate != null)
					    .FirstOrDefault();

				    sumQuantity = execution.Promotion.PromoActions
					    .Where(x => x.Type == PromoActionType.MoreTravel)
					    .Sum(x => (int?)x.Quantity) ?? 0;
			    }

			    var payIn = "PY";
			    var script = GetScriptSearch(operation.Script);
			    var operationscript = operation.Script.FromJson();
			    var device = operation.Device.FromJson();
			    var uid = operation.Uid == null ? 0 : (long)operation.Uid;
			    var operationId = payIn + operation.Id;
			    var EMTDate = EmtService.FormatDateEMT((DateTime)operation.ConfirmationDate);
			    var ticket = operation.TicketId;
			    var operationCode = EmtService.OperationCodeEMT(operation);
			    var amount = (operation.OperationType == OperationType.Revoke) ? Convert.ToInt32(operation.Price.Price * -100) : Convert.ToInt32(operation.Price.Price * 100);
			    var cellularNumber = device == null ? "" : device.suscriberId;
			    var saldoViajes = EmtService.GetSaldoViajes(script);
			    var validity = EmtService.GetValidity(script);
			    var ampliated = EmtService.GetAmpliated(script);
			    var version = operation.Price.Version;
			    var cod_viajero = EmtService.GetCodViajero(script);
			    var cod_anulacion = (lastRecharge.Id == 0) ? "" : payIn + lastRecharge.Id.ToString();
			    var cardType = script.Card.Tarjeta.Tipo.Value.ToString();
			    var cardSubType = Convert.ToString((int)script.Card.Tarjeta.Tipo.Value) + Convert.ToString((int)script.Card.Tarjeta.Subtipo.Value);

#if TEST || HOMO || DEBUG || EMULATOR
			    var operationType = "S";
#else
			    var  operationType = "R";
#endif

			    string entry = "18," + operationId + "," + EMTDate + "," + ticket + "," + uid + "," + operationCode + "," + amount + ",0,96,A," + cellularNumber + ",0," + saldoViajes + "," + validity + "," + ampliated + "," + version + "," + cod_viajero + "," + cod_anulacion + "," + "11" + ",1," + operationType + "," + payIn + "," + sumQuantity + Environment.NewLine;

                //Revisar el tipo de script
                result.Result = (await EmtService.PostEMTFileOperationsAsync(entry))
                    .ToString();
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e.Message;
            }

            return new ServerResult[] { result };
        }
        #endregion Revoked

        #region Refund
        public async Task<IEnumerable<ServerResult>> Refund(TransportOperation operation, Payment payment, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                // De momento no hay refund
                return new ServerResult[] { };
            });
        }
        #endregion Refund

        #region WhiteListDownload
        public async Task<IEnumerable<WhiteListDto>> WhiteListDownload(DateTime now)
        {
            return await Task.Run(() => new List<WhiteListDto>());
        }
        #endregion WhiteListDownload

        #region WhiteListMark
        public Task<string> WhiteListMark(WhiteList whiteItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            throw new NotImplementedException();
        }
        #endregion WhiteListMark

        #region WhiteListUnmark
        public Task<string> WhiteListUnmark(WhiteList whiteItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            throw new NotImplementedException();
        }
        #endregion WhiteListUnmark

        #region GreyListDownload
        public async Task<IEnumerable<GreyListDto>> GreyListDownload(DateTime now)
        {
            return await Task.Run(() => new List<GreyListDto>());
        }
        #endregion GreyListDownload

        #region GreyListMark
        public Task<string> GreyListMark(GreyList greyItem, TransportOperation operation, string mobileSerial, DateTime now)
		{
			throw new NotImplementedException();
		}
		#endregion GreyListMark

		#region GreyListUnmark
		public async Task<string> GreyListUnmark(TransportOperation operation, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                return "";
            });
        }
        #endregion GreyListUnmark

        #region BlackListDownload
        public async Task<IEnumerable<BlackListDto>> BlackListDownload(DateTime now)
        {
            return await Task.Run(() => new List<BlackListDto>());
        }
        #endregion BlackListDownload

        #region BlackListMark
        public Task<string> BlackListMark(BlackList blackList, TransportOperation operation, string mobileSerial, DateTime now)
		{
			throw new NotImplementedException();
		}
		#endregion BlackListMark

		#region BlackListUnmark
		public async Task<string> BlackListUnmark(TransportOperation operation, string mobileSerial, DateTime now)
		{
            return await Task.Run(() =>
            {
                return "";
            });
		}
		#endregion BlackListUnmark

		#region GetScriptSearch
		private dynamic GetScriptSearch(string scriptRequest)
		{
			var arguments = scriptRequest.FromJson();
			return arguments;
		}
        #endregion GetScriptSearch
    }
}
