using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceZone
{
	public partial class ServiceZoneUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int ConcessionId { get; private set; }
		public string Name { get; private set; }
		public decimal? CancelationAmount { get; private set; }

		#region Constructors
		public ServiceZoneUpdateArguments(int id, int concessionId, string name, decimal? cancelationAmount)
		{
			Id = id;
			ConcessionId = concessionId;
			Name = name;
			CancelationAmount = cancelationAmount;
		}
		#endregion Constructors
	}
}
