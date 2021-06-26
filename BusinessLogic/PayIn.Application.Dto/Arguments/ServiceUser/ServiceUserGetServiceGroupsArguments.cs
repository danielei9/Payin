using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceUserGetServiceGroupsArguments : IArgumentsBase
	{
		public int UserId { get; set; }

		#region Constructors
		public ServiceUserGetServiceGroupsArguments(int userId)
		{
			UserId = userId;
		}
		#endregion Constructors
	}
}
