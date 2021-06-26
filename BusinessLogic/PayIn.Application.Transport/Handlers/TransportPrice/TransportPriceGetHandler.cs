using PayIn.BusinessLogic.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Transport;
using PayIn.Application.Dto.Transport.Results.TransportPrice;
using PayIn.Application.Dto.Transport.Arguments.TransportPrice;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportPriceGetHandler :
		IQueryBaseHandler<TransportPriceGetArguments, TransportPriceGetResult>
	{
		private readonly IEntityRepository<TransportPrice> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public TransportPriceGetHandler
		(
			IEntityRepository<TransportPrice> repository,
			ISessionData sessionData
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportPriceGetResult>> ExecuteAsync(TransportPriceGetArguments arguments)
		{
			var item = (await Repository.GetAsync())
			.Where(x => x.Id == arguments.Id)
			.Select(x => new
			{
				Id = x.Id,
				Start = x.Start,
				End = x.End,
				Version = x.Version,
				Price = x.Price,				
				Zone = x.Zone,				
				MaxTimeChanges = x.MaxTimeChanges,
				OperatorContext = x.OperatorContext

			})
				.ToList()
				.Select(x => new TransportPriceGetResult
				{
					Id = x.Id,
					Start = x.Start,
					End = x.End,
					Version = x.Version,
					Price = x.Price,
					Zone = x.Zone,
					ZoneAlias = ((x.Zone == null) ? "" : x.Zone.ToEnumAlias()),					
					MaxTimeChanges = x.MaxTimeChanges,
					OperatorContext = x.OperatorContext
				});

			return new ResultBase<TransportPriceGetResult> { Data = item };
		}
		#endregion ExecuteAsync
	}
}
