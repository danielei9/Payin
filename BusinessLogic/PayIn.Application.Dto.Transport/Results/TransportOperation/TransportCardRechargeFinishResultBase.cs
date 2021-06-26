using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportCard
{
	public class TransportCardRechargeFinishResultBase : ResultBase<TransportCardRechargeFinishResult>
	{
		public int TicketId { get; set; }
	}
}
