using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductRemoveGroupArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int GroupId { get; set; }

		#region Constructors
		public ProductRemoveGroupArguments(int groupId)
		{
			GroupId = groupId;
		}
		#endregion Constructors
	}
}
