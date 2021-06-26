using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserGetAllByDashBoardArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceUserGetAllByDashBoardArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
