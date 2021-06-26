using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceDocumentGetCreateResultBase : ResultBase<ServiceDocumentGetCreateResult>
	{
		public IEnumerable<SelectorResult> SystemCardId { get; set; }
	}
}