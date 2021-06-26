using PayIn.Application.Dto.Results;
using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Transport.Results.TransportCard
{
	public class TransportCardGetByDeviceAddressResultBase : ResultBase<TransportCardGetByDeviceAddressResult>
	{
		public IEnumerable<ServiceCardReadInfoResult_RechargeTitle> RechargeTitles { get; set; }
	}
}
