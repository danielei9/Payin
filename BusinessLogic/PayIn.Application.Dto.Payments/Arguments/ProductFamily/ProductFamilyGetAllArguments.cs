using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductFamilyGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ProductFamilyGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
