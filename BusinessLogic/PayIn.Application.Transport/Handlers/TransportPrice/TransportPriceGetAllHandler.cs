using PayIn.Application.Dto.Transport.Arguments.TransportPrice;
using PayIn.Application.Dto.Transport.Results.TransportPrice;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportPriceGetAllHandler : IQueryBaseHandler<TransportPriceGetAllArguments, TransportPriceGetAllResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportPrice> Repository;

		#region Constructors
		public TransportPriceGetAllHandler(
			ISessionData sessionData,
			IEntityRepository<TransportPrice> repository)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportPriceGetAllResult>> ExecuteAsync(TransportPriceGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.TransportTitleId == arguments.TitleId) &&
					(x.State == TransportPriceState.Active) &&
					x.Title.State == TransportTitleState.Active
				);

			var result = items
				.OrderBy(x => x.Zone)
				.Select(x => new
				{
					Id = x.Id,
					Start = x.Start,
					End = x.End,
					Version = x.Version,
					Price = x.Price,
					Zone = x.Zone,
					State = x.State
				})
				.ToList()
				.Select(x => new TransportPriceGetAllResult
				{
					Id = x.Id,
					Start = x.Start.ToUTC(),
					End = x.End.ToUTC(),
					Version = x.Version,
					Price = x.Price,
					ZoneAlias = ((x.Zone == null) ? "" : x.Zone.ToEnumAlias()),
					State = x.State
				})
				.OrderByDescending(x => x.Version).ThenBy(x => x.ZoneAlias);
			
			return new ResultBase<TransportPriceGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

