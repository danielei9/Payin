using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTag
{
    public partial class ServiceTagGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceTagGetSelectorArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
