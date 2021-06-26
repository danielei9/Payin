using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceFreeDays
{
	public partial class ServiceFreeDaysDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceFreeDaysDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
