using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionGetSelectorMembersArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceConcessionGetSelectorMembersArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}
