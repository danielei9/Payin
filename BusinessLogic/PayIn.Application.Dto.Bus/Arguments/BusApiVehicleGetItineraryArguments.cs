using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiVehicleGetItineraryArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public string Filter { get; set; }

		#region Constructors
		public BusApiVehicleGetItineraryArguments(string filter)
		{
			this.Filter = filter;
		}
		#endregion Constructors
	}
}
