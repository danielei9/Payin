using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceZone
{
	public partial class ServiceZoneCreateArguments : IArgumentsBase
	{
		public int ConcessionId { get; private set; }
		public string Name { get; private set; }
		public decimal? CancelationAmount { get; private set; }

		#region Constructors
		public ServiceZoneCreateArguments(int concessionId, string name, decimal? cancelationAmount)
		{
			ConcessionId = concessionId;
			Name = name;
			CancelationAmount = cancelationAmount;
		}
		#endregion Constructors
	}
}
