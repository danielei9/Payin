using PayIn.Application.Dto.Internal.Results;
using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PublicTicketCreateAndGetResultBase : ResultBase<MobileTicketCreateAndGetResult>
	{
		public IEnumerable<MobilePaymentMediaGetAllResult> PaymentMedias { get; set; }
		public PaymentMediaCreateWebCardResult Result { get; set; }
	}
}
