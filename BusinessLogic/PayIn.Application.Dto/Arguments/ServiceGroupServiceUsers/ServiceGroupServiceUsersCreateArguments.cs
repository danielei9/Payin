using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceGroupServiceUsersCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.transportCardSupportTitleCompatibility.transportCardSupportId")]
		[Required]
		public int UserId { get; set; }
		public int GroupId { get; set; }

		#region Constructors
		public ServiceGroupServiceUsersCreateArguments(int userId, int groupId)
		{
			UserId = userId;
			GroupId = groupId;
		}
		#endregion Constructors
	}
}