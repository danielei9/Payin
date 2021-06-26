using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceIncidenceGetArguments_Notifications : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceIncidenceGetArguments_Notifications(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
