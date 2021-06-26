using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductGroupsGetAllArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ProductGroupsGetAllArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
