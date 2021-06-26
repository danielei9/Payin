using System.Collections.Generic;
using PayIn.Application.Dto.Payments.Results;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeRechargeGetResultBase : ResultBase<EntranceTypeRechargeGetResult>
	{
		public IEnumerable<SelectorResult> PurseId;
	}
}
