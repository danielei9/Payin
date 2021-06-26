using PayIn.Common;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public class ServiceUserGetServiceGroupsResultBase : ResultBase<ServiceUserGetServiceGroupsResult>
	{
		public string UserName { get; set; }
	}
}
