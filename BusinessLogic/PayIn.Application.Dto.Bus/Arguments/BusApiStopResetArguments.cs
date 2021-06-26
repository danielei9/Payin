using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public partial class BusApiStopResetArguments : IArgumentsBase
	{
		public int VehicleId { get; set; }

		#region Contructors
		public BusApiStopResetArguments(int vehicleId)
		{
			this.VehicleId = vehicleId;
		}
		#endregion Contructors
	}
}
