using Xp.Application.Dto;

namespace PayIn.Application.Dto.Transport.Results.TransportCard
{
	public class TransportCardRechargeResultBase : ResultBase<TransportCardRechargeResult>
	{
		public int TicketId { get; set; }
	}
}
