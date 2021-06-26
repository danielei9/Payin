using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductDeleteGroupArguments : IArgumentsBase
	{
		public int Id		{ get; set; }
		public int GroupId	{ get; set; }

		#region Constructors
		public ProductDeleteGroupArguments(int id, int groupId)
		{
			Id = id;
			GroupId = groupId;
		}
		#endregion Constructors
	}
}
