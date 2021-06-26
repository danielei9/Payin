using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceUserCreateCardSelectGetResultBase : ResultBase<ServiceUserCreateCardSelectGetResult>
	{
		public IEnumerable<SelectorResult> SystemCardId { get; set; }
	}
}
