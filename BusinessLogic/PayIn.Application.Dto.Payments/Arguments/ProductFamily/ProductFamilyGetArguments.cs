using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductFamilyGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ProductFamilyGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
