using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTimeTable
{
	public partial class ServiceTimeTableGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceTimeTableGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
