using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Bus;
using PayIn.Domain.Bus.Enums;
using System;
using System.Threading.Tasks;
using System.Linq;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusApiRequestCreateHandler : IServiceBaseHandler<BusApiRequestCreateArguments>
	{
        [Dependency] public IEntityRepository<Request> Repository { get; set; }
        [Dependency] public IEntityRepository<RequestStop> RequestStopRepository { get; set; }
		[Dependency] public IEntityRepository<Stop> StopRepository { get; set; }
		[Dependency] public IEntityRepository<TimeTable> TimeTableRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(BusApiRequestCreateArguments arguments)
		{
			var now = DateTime.UtcNow;
			var time = now.TimeOfDay;

			// Check Timetable
			var inTimeTable = (await TimeTableRepository.GetAsync())
				.Where(x => x.LineId == arguments.LineId)
				.ToList()
				.Where(x =>
					((x.Since.TimeOfDay <= x.Until.TimeOfDay) && (x.Since.TimeOfDay <= time) && (x.Until.TimeOfDay >= time)) ||
					((x.Since.TimeOfDay >= x.Until.TimeOfDay) && ((x.Since.TimeOfDay <= time) || (x.Until.TimeOfDay >= time)))
				)
				.Any();
			if (!inTimeTable)
				throw new ApplicationException("Petición fuera de horario.");

			// Calcular sentido en paradas desdobladas
			var stopIds = await CheckStopSense(arguments.FromId, arguments.ToId);
			var fromId = stopIds.Item1;
			var toId = stopIds.Item2;

			// From
			var from = new RequestStop
            {
                StopId = fromId,
                State = RequestNodeState.Active
            };
            await RequestStopRepository.AddAsync(from);

			// To
            var to = new RequestStop
            {
                StopId = toId,
                State = RequestNodeState.Active
            };
            await RequestStopRepository.AddAsync(to);
			
			// Request
			var request = new Request
            {
                Login = SessionData.Login,
				UserName = arguments.UserName,
				Timestamp = now,
                From = from,
                To = to
            };
            await Repository.AddAsync(request);

            return request;
		}
		#endregion ExecuteAsync

		#region CheckStopSense
		public async Task<Tuple<int, int>> CheckStopSense(int fromId, int toId)
		{
			var stops = (await StopRepository.GetAsync());

			// Obtener Master Codes
			var fromMasterCode = stops
				.Where(y => y.Id == fromId)
				.Select(y => y.MasterCode)
				.FirstOrDefault();
			var toMasterCode = stops
				.Where(y => y.Id == toId)
				.Select(y => y.MasterCode)
				.FirstOrDefault();

			// Buscar paradas
			var fromStops = stops
				.Where(x => x.MasterCode == fromMasterCode)
				.ToList();
			var toStops = stops
				.Where(x => x.MasterCode == toMasterCode)
				.ToList();

			// Obtener los orders
			var fromStopOrder = fromStops.FirstOrDefault()?.Order;
			var toStopOrder = toStops.FirstOrDefault()?.Order;

			if (toStopOrder < fromStopOrder)
			{
				if (fromStops.Count() > 1)
				{
					fromId =
						fromStops
							.Where(x => x.Code.Right(1) == ".")
							.Select(x => (int?)x.Id)
							.FirstOrDefault() ??
						fromId;
				}

				if (toStops.Count() > 1)
				{
					toId =
					toStops
						.Where(x => x.Code.Right(1) == ".")
						.Select(x => (int?)x.Id)
						.FirstOrDefault() ??
					toId;
				}
			}

			return new Tuple<int, int>(fromId, toId);
		}
		#endregion IsInversCheckStopSensee
	}
}
