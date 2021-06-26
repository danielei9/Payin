using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceCardGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
