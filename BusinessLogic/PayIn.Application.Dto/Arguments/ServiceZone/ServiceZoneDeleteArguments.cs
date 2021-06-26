using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceZone
{
	public partial class ServiceZoneDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceZoneDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
