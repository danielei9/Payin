using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.Application.Dto.Payments.Results.Shipment;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Payments.Shipment;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ShipmentGetHandler :
		IQueryBaseHandler<ShipmentGetArguments, ShipmentGetResult>
	{
		private readonly IEntityRepository<serV> _Repository;
		private readonly ISessionData _SessionData;

		#region Constructors
		public ShipmentGetHandler(IEntityRepository<serV> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			_SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ShipmentGetResult>> ExecuteAsync(ShipmentGetArguments arguments)
		{
			var items = (await _Repository.GetAsync());
			var now = DateTime.Now;
			var nowUTC = now.ToUTC();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new 
				{
					Id = x.Id,
					Since = x.Since,
					Until = x.Until,
					Amount = x.Amount,
					Name = x.Name,
					Started = (x.Since <= now) ? true : false,
					Finished = (x.Until <= now) ? true : false,
				})
				.OrderByDescending(x => x.Since)
				.ToList()
				.Select(x => new ShipmentGetResult
				{
					Id = x.Id,
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC(),
					Amount = x.Amount,
					Name = x.Name,
					Started = x.Started,
					Finished = x.Finished
				})
				.ToList()
			;
			return new ResultBase<ShipmentGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
