using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceGroupServiceUsersGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public ServiceGroupServiceUsersGetSelectorArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
