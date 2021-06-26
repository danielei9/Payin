using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.Domain.Bus;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class BusApiStopCreateHandler : IServiceBaseHandler<BusApiStopCreateArguments>
    {
        [Dependency] public IEntityRepository<Stop> Repository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(BusApiStopCreateArguments arguments)
        {
			var waitingTime = new TimeSpan(0,0,arguments.WaitingTime_Seconds);

            var item = new Stop();
			item.Code = arguments.Code;
			item.MasterCode = arguments.MasterCode;
			item.Name = arguments.Name;
			item.Location = arguments.Location;
            item.GeofenceRadious = arguments.GeofenceRadious;
            item.Longitude = arguments.Longitude;
            item.Latitude = arguments.Latitude;
			item.WaitingTime = waitingTime;
            item.LineId = arguments.LineId; 
            item.Type  = arguments.Type;  

            await Repository.AddAsync(item);

			return item;
        }
        #endregion ExecuteAsync
    }
}
