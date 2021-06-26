using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceGroupGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceGroupGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
