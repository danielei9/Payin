using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTimeTable
{
	public partial class ServiceTimeTableDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceTimeTableDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
