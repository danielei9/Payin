using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServicePrice
{
	public partial class ServicePriceGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServicePriceGetArguments(int id)
		{
			Id = id;
		}
		public ServicePriceGetArguments()
		{
		}
		#endregion Constructors
	}
}
