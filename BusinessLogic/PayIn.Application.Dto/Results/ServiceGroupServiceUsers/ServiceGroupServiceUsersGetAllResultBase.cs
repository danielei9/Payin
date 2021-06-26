using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public class ServiceGroupServiceUsersGetAllResultBase : ResultBase<ServiceGroupServiceUsersGetAllResult>
	{
		public string GroupName { get; set; }
	}
}
