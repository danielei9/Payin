using PayIn.Application.Dto.Transport.Arguments.TransportSystem;
using PayIn.Application.Dto.Transport.Results.TransportSystem;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common;
using PayIn.Domain.Transport;

namespace PayIn.Application.Public.Handlers
{
	public class TransportSystemGetAllHandler :
		IQueryBaseHandler<TransportSystemGetAllArguments, TransportSystemGetAllResult>
	{
		private readonly IEntityRepository<TransportSystem> Repository;

		#region Constructors
		public TransportSystemGetAllHandler(
			IEntityRepository<TransportSystem> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<TransportSystemGetAllResult>> IQueryBaseHandler<TransportSystemGetAllArguments, TransportSystemGetAllResult>.ExecuteAsync(TransportSystemGetAllArguments arguments)
		{
			var systems = await Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				systems = systems.Where(x => (
					x.Name.Contains(arguments.Filter)
				));

			var result = systems
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					CardType = x.CardType,
					State = x.State				
				})
				.ToList()
				.Select(x => new TransportSystemGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					CardType = x.CardType.ToString(),
					State = (int)x.State
				})
				.OrderBy(x => x.Id);

			return new ResultBase<TransportSystemGetAllResult> { Data = result };
		}
		#endregion ExecuteAsyn	
	}
}
