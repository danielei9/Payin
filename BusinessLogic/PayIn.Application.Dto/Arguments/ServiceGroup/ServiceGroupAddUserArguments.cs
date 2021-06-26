using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceGroupAddUserArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceGroup.user")]		public int ServiceUserId	{ get; set; }
		[Display(Name = "resources.serviceGroup.group")]	public int ServiceGroupId { get; set; }

		#region Constructors
		public ServiceGroupAddUserArguments(int serviceUserId, int serviceGroupId)
		{
			ServiceUserId = serviceUserId;
			ServiceGroupId = serviceGroupId;
		}
		#endregion Constructors
	}
}
