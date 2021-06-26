using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Domain.Bus;
using PayIn.Domain.Bus.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusApiStopResetHandler : IServiceBaseHandler<BusApiStopResetArguments>
	{
		[Dependency] public IEntityRepository<Vehicle> VehicleRepository { get; set; }
		[Dependency] public BusApiVehicleGetItineraryHandler BusApiVehicleGetItineraryHandler { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(BusApiStopResetArguments arguments)
		{
			//var now = DateTime.UtcNow;

			// Get Vehicle
			var vehicle = (await VehicleRepository.GetAsync())
				.Where(x => x.Id == arguments.VehicleId)
				.FirstOrDefault()
				?? throw new ArgumentNullException(nameof(arguments.VehicleId));
			vehicle.CurrentStopId = null;
			await UnitOfWork.SaveAsync();

			// Get itinerary
			var data = (await BusApiVehicleGetItineraryHandler.ExecuteInternalAsync(vehicle.LineId.Value));
			while (data.Stops.Count() > 0)
			{
				var previousStop = data.Stops.First();
				data.Stops = data.Stops.Skip(1);
			}
			if (data.Stops.Count() == 0)
				return (await BusApiVehicleGetItineraryHandler.ExecuteInternalAsync(vehicle.LineId.Value));

			vehicle.CurrentStopId = data.Stops.FirstOrDefault()?.Id;
			vehicle.CurrentSense = data.Stops.FirstOrDefault()?.Sense ?? RouteSense.None;

			return data;
		}
		#endregion ExecuteAsync
	}
}
