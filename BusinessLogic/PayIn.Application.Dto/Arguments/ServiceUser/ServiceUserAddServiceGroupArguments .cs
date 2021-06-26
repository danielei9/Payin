using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserAddServiceGroupArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceUser.user")]
		[Required] public int ServiceUserId { get; set; }
		[Display(Name = "resources.serviceUser.group")]
		[Required] public int ServiceGroupId { get; set; }

		#region Constructors
		public ServiceUserAddServiceGroupArguments(int serviceUserId, int serviceGroupId)
		{
			ServiceUserId = serviceUserId;
			ServiceGroupId = serviceGroupId;
		}
		#endregion Constructors
	}
}
