using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductFamilyUpdateGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ProductFamilyUpdateGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
