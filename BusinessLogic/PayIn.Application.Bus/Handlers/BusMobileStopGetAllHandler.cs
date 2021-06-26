using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Bus;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusMobileStopGetAllHandler : IQueryBaseHandler<BusMobileStopGetAllArguments, BusMobileStopGetAllResult>
	{
		[Dependency] public IEntityRepository<Stop> StopRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<BusMobileStopGetAllResult>> ExecuteAsync(BusMobileStopGetAllArguments arguments)
		{
			var result = (await StopRepository.GetAsync())
				.Where(x =>
					x.Line.Login == SessionData.Login
				)
				.Select(x => new BusMobileStopGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Longitude = x.Longitude,
					Latitude = x.Latitude,
					Radius = 50
				})
				.ToList();
			
			return new ResultBase<BusMobileStopGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
