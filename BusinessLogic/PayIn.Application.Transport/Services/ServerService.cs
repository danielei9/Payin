using Microsoft.Practices.Unity;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayIn.Application.Transport.Services
{
    public class ServerService : IServerService
	{
		[Dependency] public FgvServerService FgvServerService { get; set; }
		[Dependency] public EigeServerService EigeServerService { get; set; }
		[Dependency] public EmtServerService EmtServerService { get; set; }

		#region Recharged
		public async Task<IEnumerable<ServerResult>> Recharged(TransportOperation operation, Payment payment, string mobileSerial, DateTime now)
		{
            if (operation.Price.Title.Environment == Domain.Transport.Eige.Enums.EigeCodigoEntornoTarjetaEnum.Valencia)
            {
                if (operation.Price.Title.OwnerCode == EigeService.FgvValenciaCode)
                    return await FgvServerService.Recharged(operation, payment, mobileSerial, now);
                else if (operation.Price.Title.OwnerCode == EigeService.EigeCode)
                    return await EigeServerService.Recharged(operation, payment, mobileSerial, now);
                else if (operation.Price.Title.OwnerCode == EigeService.EmtCode)
                    return await EmtServerService.Recharged(operation, payment, mobileSerial, now);
            }
            else if (operation.Price.Title.Environment == Domain.Transport.Eige.Enums.EigeCodigoEntornoTarjetaEnum.Alicante)
            {
                if (operation.Price.Title.OwnerCode == EigeService.FgvAlicanteCode)
                    return await FgvServerService.Recharged(operation, payment, mobileSerial, now);
            }

            throw new NotImplementedException();
        }
        #endregion Recharged

        #region Revoked
        public async Task<IEnumerable<ServerResult>> Revoked(TransportOperation operation, string mobileSerial, DateTime now)
        {
            if (operation.Price.Title.Environment == Domain.Transport.Eige.Enums.EigeCodigoEntornoTarjetaEnum.Valencia)
            {
                if (operation.Price.Title.OwnerCode == EigeService.FgvValenciaCode)
                    return await FgvServerService.Revoked(operation, mobileSerial, now);
                else if (operation.Price.Title.OwnerCode == EigeService.EigeCode)
                    return await EigeServerService.Revoked(operation, mobileSerial, now);
                else if (operation.Price.Title.OwnerCode == EigeService.EmtCode)
                    return await EmtServerService.Revoked(operation, mobileSerial, now);
            }
            else if (operation.Price.Title.Environment == Domain.Transport.Eige.Enums.EigeCodigoEntornoTarjetaEnum.Alicante)
            {
                if (operation.Price.Title.OwnerCode == EigeService.FgvAlicanteCode)
                    return await FgvServerService.Revoked(operation, mobileSerial, now);
            }

            throw new NotImplementedException();
        }
        #endregion Revoked

        #region Refund
        public async Task<IEnumerable<ServerResult>> Refund(TransportOperation operation, Payment payment, string mobileSerial, DateTime now)
        {
            if (operation.Price.Title.Environment == Domain.Transport.Eige.Enums.EigeCodigoEntornoTarjetaEnum.Valencia)
            {
                if (operation.Price.Title.OwnerCode == EigeService.FgvValenciaCode)
                    return await FgvServerService.Refund(operation, payment, mobileSerial, now);
                else if (operation.Price.Title.OwnerCode == EigeService.EigeCode)
                    return await EigeServerService.Refund(operation, payment, mobileSerial, now);
                else if (operation.Price.Title.OwnerCode == EigeService.EmtCode)
                    return await EmtServerService.Refund(operation, payment, mobileSerial, now);
            }
            else if (operation.Price.Title.Environment == Domain.Transport.Eige.Enums.EigeCodigoEntornoTarjetaEnum.Alicante)
            {
                if (operation.Price.Title.OwnerCode == EigeService.FgvAlicanteCode)
                    return await FgvServerService.Refund(operation, payment, mobileSerial, now);
            }

            throw new NotImplementedException();
        }
        #endregion Refund

        #region WhiteListDownload
        public async Task<IEnumerable<WhiteListDto>> WhiteListDownload(DateTime now)
        {
            var resultEige = new List<WhiteListDto>(); // await EigeServerService.WhiteListDownload(now);
            var resultEmt = new List<WhiteListDto>(); // await EmtServerService.WhiteListDownload(now);
            var resultFgv = await FgvServerService.WhiteListDownload(now);

            return
                (resultEige ?? new List<WhiteListDto>())
                .Union(resultEmt ?? new List<WhiteListDto>())
                .Union(resultFgv ?? new List<WhiteListDto>());
        }
        #endregion WhiteListDownload

        #region WhiteListMark
        public async Task<string> WhiteListMark(WhiteList whiteItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            if (whiteItem.Source == WhiteList.WhiteListSourceType.Payin)
                throw new NotImplementedException();
            else if (whiteItem.Source == WhiteList.WhiteListSourceType.SigAPunt)
                throw new NotImplementedException();
            else if ((whiteItem.Source == WhiteList.WhiteListSourceType.FgvValencia) || (whiteItem.Source == WhiteList.WhiteListSourceType.FgvAlacant))
                return await FgvServerService.WhiteListMark(whiteItem, operation, mobileSerial, now);
            else
                throw new NotImplementedException();
        }
        #endregion WhiteListMark

        #region WhiteListUnmark
        public async Task<string> WhiteListUnmark(WhiteList whiteItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            if (whiteItem.Source == WhiteList.WhiteListSourceType.Payin)
                throw new NotImplementedException();
            else if (whiteItem.Source == WhiteList.WhiteListSourceType.SigAPunt)
                throw new NotImplementedException();
            else if ((whiteItem.Source == WhiteList.WhiteListSourceType.FgvValencia) || (whiteItem.Source == WhiteList.WhiteListSourceType.FgvAlacant))
                return await FgvServerService.WhiteListUnmark(whiteItem, operation, mobileSerial, now);
            else
                throw new NotImplementedException();
        }
        #endregion WhiteListUnmark

        #region GreyListDownload
        public async Task<IEnumerable<GreyListDto>> GreyListDownload(DateTime now)
        {
            var resultEige = new List<GreyListDto>(); // await EigeServerService.GreyListDownload(now);
            var resultEmt = new List<GreyListDto>(); // await EmtServerService.GreyListDownload(now);
            var resultFgv = await FgvServerService.GreyListDownload(now);

            return
                (resultEige ?? new List<GreyListDto>())
                .Union(resultEmt ?? new List<GreyListDto>())
                .Union(resultFgv ?? new List<GreyListDto>());
        }
        #endregion GreyListDownload

        #region GreyListMark
        public async Task<string> GreyListMark(GreyList greyItem, TransportOperation operation, string mobileSerial, DateTime now)
		{
            greyItem.IsConfirmed = true;

            if (greyItem.Source == GreyList.GreyListSourceType.Payin)
            {
                greyItem.ResolutionDate = now;
                greyItem.Resolved = true;

                return "OK";
            }
            else if (greyItem.Source == GreyList.GreyListSourceType.SigAPunt)
                return await EigeServerService.GreyListMark(greyItem, operation, mobileSerial, now);
            else if ((greyItem.Source == GreyList.GreyListSourceType.FgvValencia) || (greyItem.Source == GreyList.GreyListSourceType.FgvAlacant))
                return await FgvServerService.GreyListMark(greyItem, operation, mobileSerial, now);
            else
                throw new NotImplementedException();
        }
		#endregion GreyListMark

		#region GreyListUnmark
		public async Task<string> GreyListUnmark(TransportOperation operation, string mobileSerial, DateTime now)
        {
            var result = "";
            {
                var temp = await EigeServerService.GreyListUnmark(operation, mobileSerial, now);
                result += temp.IsNullOrEmpty() ? "" : "SigApunT: " + temp;
            }
            {
                var temp = await FgvServerService.GreyListUnmark(operation, mobileSerial, now);
                result += temp.IsNullOrEmpty() ? "" : "FGV: " + temp;
            }
            {
                var temp = await EmtServerService.GreyListUnmark(operation, mobileSerial, now);
                result += temp.IsNullOrEmpty() ? "" : "EMT: " + temp;
            }
            return result;
		}
        #endregion GreyListUnmark

        #region BlackListDownload
        public async Task<IEnumerable<BlackListDto>> BlackListDownload(DateTime now)
        {
            var resultEige = new List<BlackListDto>();// await EigeServerService.BlackListDownload(now);
            var resultEmt = new List<BlackListDto>();// await EmtServerService.BlackListDownload(now);
            var resultFgv = await FgvServerService.BlackListDownload(now);

            return
                (resultEige ?? new List<BlackListDto>())
                .Union(resultEmt ?? new List<BlackListDto>())
                .Union(resultFgv ?? new List<BlackListDto>());
        }
        #endregion BlackListDownload

        #region BlackListMark
        public async Task<string> BlackListMark(BlackList blackItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            blackItem.IsConfirmed = true;

            if (blackItem.Source == BlackList.BlackListSourceType.Payin)
            {
                blackItem.ResolutionDate = now;
                blackItem.Resolved = true;

                return "OK";
            }
            else if (blackItem.Source == BlackList.BlackListSourceType.SigAPunt)
                return await EigeServerService.BlackListMark(blackItem, operation, mobileSerial, now);
            else if ((blackItem.Source == BlackList.BlackListSourceType.FgvValencia) || (blackItem.Source == BlackList.BlackListSourceType.FgvAlacant))
                return await FgvServerService.BlackListMark(blackItem, operation, mobileSerial, now);
            else
                throw new NotImplementedException();
		}
		#endregion BlackListMark

		#region BlackListUnmark
		public async Task<string> BlackListUnmark(TransportOperation operation, string mobileSerial, DateTime now)
        {
            var result = "";
            {
                var temp = await EigeServerService.BlackListUnmark(operation, mobileSerial, now);
                result += temp.IsNullOrEmpty() ? "" : "SigApunT: " + temp;
            }
            {
                var temp = await FgvServerService.BlackListUnmark(operation, mobileSerial, now);
                result += temp.IsNullOrEmpty() ? "" : "FGV: " + temp;
            }
            {
                var temp = await EmtServerService.BlackListUnmark(operation, mobileSerial, now);
                result += temp.IsNullOrEmpty() ? "" : "EMT: " + temp;
            }
            return result;
		}
        #endregion BlackListUnmark
    }
}
