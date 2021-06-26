using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductGetAllByDashboardArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ProductGetAllByDashboardArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
