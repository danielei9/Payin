using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Bus;
using PayIn.Domain.Bus.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusApiStopVisitHandler : IServiceBaseHandler<BusApiStopVisitArguments>
	{
		[Dependency] public IEntityRepository<Stop> Repository { get; set; }
		[Dependency] public IEntityRepository<Vehicle> VehicleRepository { get; set; }
		[Dependency] public IEntityRepository<Request> RequestRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public BusApiVehicleGetItineraryHandler BusApiVehicleGetItineraryHandler { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(BusApiStopVisitArguments arguments)
		{
			var now = DateTime.UtcNow;

			// Get stop
			var stop = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					x.LineId
				})
				.FirstOrDefault()
				?? throw new ArgumentNullException(nameof(arguments.Id));

			// Get Vehicle
			var vehicle = (await VehicleRepository.GetAsync())
				.Where(x => x.Id == arguments.VehicleId)
				.FirstOrDefault()
				?? throw new ArgumentNullException(nameof(arguments.VehicleId));

			// Visitar solicitudes de parada FROM
			{
				var requestFroms = (await RequestRepository.GetAsync())
					.Where(x =>
						(x.From.StopId == arguments.Id) &&
						(x.From.State == RequestNodeState.Active) &&
						(x.From.VisitTimeStamp == null)
					)
					.Select(x => x.From)
					.ToList();
				foreach (var requestStop in requestFroms)
					requestStop.VisitTimeStamp = now;
			}

			// Visitar solicitudes de parada TO
			{
				var requestTos = (await RequestRepository.GetAsync())
					.Where(x =>
						(x.To.StopId == arguments.Id) &&
						(x.To.State == RequestNodeState.Active) &&
						(x.To.VisitTimeStamp == null) &&
						(x.From.VisitTimeStamp != null)
					)
					.Select(x => x.To)
					.ToList();
				foreach (var requestStop in requestTos)
					requestStop.VisitTimeStamp = now;
			}

			vehicle.CurrentStopId = arguments.Id;
			await UnitOfWork.SaveAsync();

			// Get itinerary
			var data = (await BusApiVehicleGetItineraryHandler.ExecuteInternalAsync(stop.LineId));
			while (data.Stops.Count() > 0)
			{
				var previousStop = data.Stops.First();
				data.Stops = data.Stops.Skip(1);

				if (previousStop.Id == arguments.Id)
					break;
			}
			if (data.Stops.Count() == 0)
				return (await BusApiVehicleGetItineraryHandler.ExecuteInternalAsync(stop.LineId));

			vehicle.CurrentStopId = data.Stops.FirstOrDefault()?.Id;
			vehicle.CurrentSense = data.Stops.FirstOrDefault()?.Sense ?? RouteSense.None;

			return data;
		}
		#endregion ExecuteAsync
	}
}
