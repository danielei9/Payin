using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductMobileGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ProductMobileGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
