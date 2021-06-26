using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServicePrice
{
	public partial class ServicePriceDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServicePriceDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
