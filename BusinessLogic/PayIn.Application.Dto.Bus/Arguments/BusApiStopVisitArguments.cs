using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public partial class BusApiStopVisitArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int VehicleId { get; set; }

		#region Contructors
		public BusApiStopVisitArguments(int vehicleId)
		{
			this.VehicleId = vehicleId;
		}
		#endregion Contructors
	}
}
