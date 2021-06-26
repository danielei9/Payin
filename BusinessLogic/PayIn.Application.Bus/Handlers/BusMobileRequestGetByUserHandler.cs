using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Bus;
using PayIn.Domain.Bus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusMobileRequestGetByUserHandler : IQueryBaseHandler<BusMobileRequestGetByUserArguments, BusMobileRequestGetByUserResult>
	{
		[Dependency] public IEntityRepository<Request> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public BusApiVehicleGetItineraryHandler BusApiVehicleGetItineraryHandler { get; set; }
		[Dependency] public BusApiStopGetByLineHandler BusApiStopGetByLineHandler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<BusMobileRequestGetByUserResult>> ExecuteAsync(BusMobileRequestGetByUserArguments arguments)
		{
			var requests = (await Repository.GetAsync())
				.Where(x =>
					x.From.State == RequestNodeState.Active &&
					x.From.Stop.LineId == arguments.LineId &&
					x.To.Stop.LineId == arguments.LineId &&
					x.Login == SessionData.Login &&
					x.To.VisitTimeStamp == null
				)
				.Select(x => new
				{
					x.Id,
					FromId = x.From.StopId,
					FromCode = x.From.Stop.Code,
					FromName = x.From.Stop.Name,
					FromLocation = x.From.Stop.Location,
					FromVisitTimeStamp = x.From.VisitTimeStamp,
					FromTime = (TimeSpan?)null, // Se asigna en el bucle
					ToId = x.To.StopId,
					ToCode = x.To.Stop.Code,
					ToName = x.To.Stop.Name,
					ToLocation = x.To.Stop.Location,
					ToVisitTimeStamp = x.To.VisitTimeStamp,
					ToTime = (TimeSpan?)null, // Se asigna en el bucle
					Vehicles = x.From.Stop.Line.Buses
						.Select(y => new
						{
							y.Id
						})
				})
				.ToList();

			var result = new List<BusMobileRequestGetByUserResult>();

			foreach (var request in requests)
			{
				foreach (var vehicle in request.Vehicles)
				{
					var item = new BusMobileRequestGetByUserResult
					{
						Id = request.Id,
						FromId = request.FromId,
						FromCode = request.FromCode,
						FromName = request.FromName,
						FromLocation = request.FromLocation,
						FromVisitTimeStamp = request.FromVisitTimeStamp,
						ToId = request.ToId,
						ToCode = request.ToCode,
						ToName = request.ToName,
						ToLocation = request.ToLocation,
						ToVisitTimeStamp = request.ToVisitTimeStamp,
					};
					result.Add(item);

					// Get itinerary
					var itinerary = await BusApiVehicleGetItineraryHandler.ExecuteInternalAsync(vehicle.Id);

					// From
					var fromRequest = itinerary.Stops
						.Where(x => x.Id == request.FromId)
						.FirstOrDefault();
					item.FromTime = fromRequest?.Time;

					// To
					var toRequest = itinerary.Stops
						.Where(x => x.Id == request.ToId)
						.FirstOrDefault();
					item.ToTime = toRequest?.Time;
				}
			}

			var stops = (await BusApiStopGetByLineHandler.ExecuteAsync(new BusApiStopGetByLineArguments(arguments.LineId))).Data
				.Where(x => x.Code == x.MasterCode)
			;
			
			return new BusMobileRequestGetByUserResultBase
			{
				Data = result,
				Stops = stops
			};
		}
		#endregion ExecuteAsync
	}
}
