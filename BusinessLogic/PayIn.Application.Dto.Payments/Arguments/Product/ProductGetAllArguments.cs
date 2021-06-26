using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ProductGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
