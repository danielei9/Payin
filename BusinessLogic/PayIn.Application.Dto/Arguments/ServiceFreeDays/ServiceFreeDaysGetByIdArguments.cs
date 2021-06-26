using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceFreeDays
{
	public partial class ServiceFreeDaysGetByIdArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceFreeDaysGetByIdArguments(int id)
		{
			Id = id;
		}
		public ServiceFreeDaysGetByIdArguments()
		{
		}
		#endregion Constructors
	}
}
