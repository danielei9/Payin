using System.Collections.Generic;
using PayIn.Application.Dto.Payments.Results;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeDonateGetResultBase : ResultBase<EntranceTypeDonateGetResult>
	{
		public IEnumerable<SelectorResult> PurseId;
	}
}
