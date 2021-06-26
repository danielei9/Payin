using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceGroupServiceUsersGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int GroupId { get; set; }

		#region Constructors
		public ServiceGroupServiceUsersGetAllArguments(int groupId, string filter)
		{
			Filter = filter;
			GroupId = groupId;
		}
		#endregion Constructors 
	}
}
