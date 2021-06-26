using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayIn.Application.Transport.Services
{
    public interface IServerService
	{
		Task<IEnumerable<ServerResult>> Recharged(TransportOperation operation, Payment payment, string mobileSerial, DateTime now);
		Task<IEnumerable<ServerResult>> Revoked(TransportOperation operation, string mobileSerial, DateTime now);
        Task<IEnumerable<ServerResult>> Refund(TransportOperation operation, Payment payment, string mobileSerial, DateTime now);

        Task<IEnumerable<WhiteListDto>> WhiteListDownload(DateTime now);
        Task<string> WhiteListMark(WhiteList whiteItem, TransportOperation operation, string mobileSerial, DateTime now);
        Task<string> WhiteListUnmark(WhiteList whiteItem, TransportOperation operation, string mobileSerial, DateTime now);

        Task<IEnumerable<GreyListDto>> GreyListDownload(DateTime now);
        Task<string> GreyListMark(GreyList greyItem, TransportOperation operation, string mobileSerial, DateTime now);
        Task<string> GreyListUnmark(TransportOperation operation, string mobileSerial, DateTime now);

        Task<IEnumerable<BlackListDto>> BlackListDownload(DateTime now);
        Task<string> BlackListMark(BlackList blackList, TransportOperation operation, string mobileSerial, DateTime now);
		Task<string> BlackListUnmark(TransportOperation operation, string mobileSerial, DateTime now);
    }
}