using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ProductGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
