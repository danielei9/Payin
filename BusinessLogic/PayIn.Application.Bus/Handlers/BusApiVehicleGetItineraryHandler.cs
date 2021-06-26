using Microsoft.Practices.Unity;
using PayIn.Application.Bus.Services;
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
	public class BusApiVehicleGetItineraryHandler : IQueryBaseHandler<BusApiVehicleGetItineraryArguments, BusApiVehicleGetItineraryResult>
	{
		[Dependency] public IEntityRepository<Vehicle> VehicleRepository { get; set; }
		[Dependency] public IEntityRepository<Request> RequestRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public BusApiStopGetByLineHandler BusApiStopsGetByLineHandler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<BusApiVehicleGetItineraryResult>> ExecuteAsync(BusApiVehicleGetItineraryArguments arguments)
		{
			// Get Itinerary
			var bus = await ExecuteInternalAsync(arguments.Id);

			// Get Stops
			var stops = ((bus == null) || (bus.LineId == null)) ?
				new ResultBase<BusApiStopGetByLineResult>() :
				(await BusApiStopsGetByLineHandler.ExecuteAsync(new BusApiStopGetByLineArguments(bus.LineId.Value)));

			// Pending Requests
			var pendingRequests = (await RequestRepository.GetAsync())
				.Where(x =>
					x.To.State == Domain.Bus.Enums.RequestNodeState.Active &&
					x.To.VisitTimeStamp == null
				)
				.OrderBy(x => x.Id)
				.Select(x => new
				{
					x.Id,
					x.Login,
					x.UserName,
					x.Timestamp,
					FromStation = x.From.Stop.Name,
					FromCode = x.From.Stop.Code,
					FromState = x.From.State,
					FromVisitTimeStamp = x.From.VisitTimeStamp,
					ToStation = x.To.Stop.Name,
					ToCode = x.To.Stop.Code,
					ToState = x.To.State,
					ToVisitTimeStamp = x.To.VisitTimeStamp
				})
				.ToList()
				.Select(x => new BusApiVehicleGetItineraryResult_PendingRequest
				{
					Id = x.Id,
					Login = x.Login,
					UserName = x.UserName,
					Timestamp = x.Timestamp.ToUTC(),
					FromName = x.FromStation,
					FromCode = x.FromCode,
					FromState = x.FromState,
					FromVisitTimeStamp = x.FromVisitTimeStamp,
					ToName = x.ToStation,
					ToCode = x.ToCode,
					ToState = x.ToState,
					ToVisitTimeStamp = x.ToVisitTimeStamp
				});

			return new BusApiVehicleGetItineraryResultBase
			{
				Stops = stops.Data
					.Where(x =>
						(x.Code == x.MasterCode) &&
						(x.Type == NodeType.Stop) &&
						(
							arguments.Filter.IsNullOrEmpty() ||
							x.Code.Contains(arguments.Filter) ||
							x.Name.Contains(arguments.Filter)
						)
					),
				PendingRequests = pendingRequests
					.Where(x =>
						arguments.Filter.IsNullOrEmpty() ||
						x.FromCode.Contains(arguments.Filter) ||
						x.FromName.Contains(arguments.Filter) ||
						x.ToCode.Contains(arguments.Filter) ||
						x.ToName.Contains(arguments.Filter)
					),
				Data = new List<BusApiVehicleGetItineraryResult> { bus }
			};
		}
		#endregion ExecuteAsync

		#region ExecuteInternalAsync
		public async Task<BusApiVehicleGetItineraryResult> ExecuteInternalAsync(int id)
		{
			var bus = (await VehicleRepository.GetAsync())
				.Where(x =>
					//(x.Line.Login == SessionData.Login) &&
					(x.Id == id)
				)
				.Select(x => new
				{
					x.Id,
					x.LineId,
					x.CurrentStopId,
					x.CurrentSense,
					Stops = x.Line.Stops
						.Select(y => new RouteStop
						{
							Id = y.Id,
							Code = y.Code,
							Name = y.Name,
							Type = y.Type,
							IsDefaultStop = y.IsDefaultStop,
							IsLastStopsGo = y.IsLastStopsGo,
							IsLastStopsBack = y.IsLastStopsBack,
							Longitude = y.Longitude,
							Latitude = y.Latitude,
							GeofenceRadious = y.GeofenceRadious,
							RequestsIn = y.RequestStops
								.Where(z =>
									(z.State == RequestNodeState.Active) &&
									(z.VisitTimeStamp == null)
								)
								.Sum(z => (int?)z.RequestEnds
									.Count()
								) ?? 0,
							RequestsOut = y.RequestStops
								.Where(z =>
									(z.State == RequestNodeState.Active) &&
									(z.VisitTimeStamp == null)
								)
								.Sum(z => (int?)z.RequestStarts
									.Count()
								) ?? 0
						}),
					RequestStops = x.Line.Stops
						.SelectMany(y => y.RequestStops
							.Where(z => z.State == RequestNodeState.Active)
							.SelectMany(z => z.RequestStarts
								.Where(a => a.To.VisitTimeStamp == null)
								.Select(a => new RouteRequestStop
								{
									Id = a.Id,
									FromCode = a.From.Stop.Code,
									FromVisited = (a.From.VisitTimeStamp != null),
									ToCode = a.To.Stop.Code,
								})
							)
						)
						.OrderBy(y => y.Id)
						,
					Routes = x.Line.Routes
						.Select(y => new
						{
							y.Sense,
							Links = y.Links
								.Select(z => new
								{
									z.Weight,
									z.Time,
									FromCode = z.From.Code,
									ToCode = z.To.Code
								})
						})
				})
				.ToList()
				.Select(x => new RouteItinerate
				{
					Id = x.Id,
					LineId = x.LineId,
					CurrentSense = x.CurrentSense,
					CurrentStop = x.Stops
						.Where(y => y.Id == x.CurrentStopId).FirstOrDefault(),
					Stops = x.Stops,
					Routes = x.Routes
						.Select(y => new RouteRoute
						{
							Sense = y.Sense,
							Links = y.Links
								.Select(z => new RouteLink
								{
									Weight = z.Weight,
									Value = z.Time.TotalMinutes.ToDecimal(),
									FromCode = z.FromCode,
									ToCode = z.ToCode
								})
						}),
					RequestStops = x.RequestStops,
					LastRoutesGo = x.Stops
						.Where(y => y.IsLastStopsGo),
					LastRoutesBack = x.Stops
						.Where(y => y.IsLastStopsBack),
					DefaultStop = x.Stops
						.Where(y => y.IsDefaultStop)
						.FirstOrDefault(),
					ReverseStops = x.Stops
						.Where(y => y.Type == NodeType.ReversePoint)
				})
				.FirstOrDefault() ??
				throw new ArgumentNullException(nameof(id));
			var linksGo = bus.Routes
				.Where(x => x.Sense == RouteSense.Go)
				.SelectMany(x => x.Links)
				.Distinct()
				.ToList();
			var linksBack = bus.Routes
				.Where(x => x.Sense == RouteSense.Back)
				.SelectMany(x => x.Links)
				.Distinct()
				.ToList();
			var links = linksGo
				.Union(linksBack)
				.Distinct()
				.ToList();

			var itineraries = (new RouteItinerates())
				.CreateItinerary(bus.CurrentStop, bus.CurrentSense, linksGo, linksBack, bus.Stops, bus.DefaultStop, bus.RequestStops, bus.LastRoutesGo, bus.LastRoutesBack, bus.ReverseStops)
				;

			return new BusApiVehicleGetItineraryResult
			{
				Id = bus.Id,
				LineId = bus.LineId,
				Stops = itineraries?.Stops
					.Select(x => new BusApiVehicleGetItineraryResult_Stop
					{
						Id = x.Id,
						Code = x.Code,
						Sense = x.Sense,
						Name = x.Name,
						IsRequested = x.IsRequested,
						Type = x.Type,
						Longitude = x.Longitude,
						Latitude = x.Latitude,
						GeofenceRadious = x.GeofenceRadious,
						Time = TimeSpan.FromMinutes(x.Value.ToDouble()),
						RequestsIn = x.RequestsIn,
						RequestsOut = x.RequestsOut,
					})
			};
		}
		#endregion ExecuteInternalAsync
	}
}
