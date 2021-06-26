using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceTypeRemoveGroupArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int GroupId { get; set; }

		#region Constructors
		public EntranceTypeRemoveGroupArguments(int groupId)
		{
			GroupId = groupId;
		}
		#endregion Constructors
	}
}
