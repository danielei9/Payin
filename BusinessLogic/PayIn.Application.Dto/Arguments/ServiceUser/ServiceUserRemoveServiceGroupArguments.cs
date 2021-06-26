using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserRemoveServiceGroupArguments : IArgumentsBase
	{
		public int UserId { get; set; }
		public int GroupId { get; set; }

		#region Constructors
		public ServiceUserRemoveServiceGroupArguments(int userId, int groupId)
		{
			UserId = UserId;
			GroupId = groupId;
		}
		#endregion Constructors
	}
}

