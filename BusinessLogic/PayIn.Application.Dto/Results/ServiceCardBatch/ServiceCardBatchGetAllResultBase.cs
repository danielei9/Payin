using System.Collections.Generic;
using PayIn.Common;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardBatchGetAllResultBase : ResultBase<ServiceCardBatchGetAllResult>
	{
		//public IEnumerable<SelectorResult> SystemCards { get; set; }
		public IEnumerable<SelectorResult> SystemCards { get; set; }
		public int? SystemCardId { get; set; }
		public string SystemCardName { get; set; }
	}
}
