using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{ 
	public class ServiceGroupGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public ServiceGroupGetSelectorArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
