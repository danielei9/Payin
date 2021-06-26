using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceSupplier
{
	public partial class ServiceSupplierGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceSupplierGetSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}
