using PayIn.Application.Dto.Transport.Arguments;
using PayIn.Application.Dto.Transport.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportPriceGetAllHandler:	IQueryBaseHandler<TransportPriceGetAllArguments, TransportPriceGetAllResult>
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
			var result = (await Repository.GetAsync())
						.Where(x => x.Title.Code == arguments.Title.Code)
						.Select(x => new TransportPriceGetAllResult
						{
							Start = x.Start,
							End = x.End,
							Version = x.Version,
							Price = x.Price,
							Quantity = x.Quantity.Value,
							TransportTitleId = x.TransportTitleId,
							Title = x.Title
						})
						.ToList();//Test

			return new ResultBase<TransportPriceGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
