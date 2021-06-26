using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceUserGetSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}
