using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceZone
{
	public partial class ServiceZoneGetByConcessionArguments : IArgumentsBase
	{
		public int ConcessionId { get; private set; }

		#region Constructors
		public ServiceZoneGetByConcessionArguments(int concessionId)
		{
			ConcessionId = concessionId;
		}
		#endregion Constructors
	}
}
