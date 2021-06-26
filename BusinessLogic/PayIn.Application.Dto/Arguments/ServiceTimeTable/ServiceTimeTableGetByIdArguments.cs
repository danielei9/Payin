using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTimeTable
{
	public partial class ServiceTimeTableGetByIdArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceTimeTableGetByIdArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
