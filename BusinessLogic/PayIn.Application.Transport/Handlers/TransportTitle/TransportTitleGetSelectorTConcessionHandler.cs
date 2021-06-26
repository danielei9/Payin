using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
using PayIn.Application.Dto.Transport.Results.TransportTitle;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportTitleGetSelectorTConcessionHandler :
		IQueryBaseHandler<TransportTitleGetSelectorTConcessionArguments, TransportTitleGetSelectorTConcessionResult>
	{
		private readonly IEntityRepository<TransportConcession> Repository;

		#region Constructors
		public TransportTitleGetSelectorTConcessionHandler(IEntityRepository<TransportConcession> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportTitleGetSelectorTConcessionResult>> ExecuteAsync(TransportTitleGetSelectorTConcessionArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.Concession.Concession.Name.Contains(arguments.Filter))
				.Select(x => new TransportTitleGetSelectorTConcessionResult
				 {
					Id = x.Id,
					Value = x.Concession.Concession.Name
				});

			return new ResultBase<TransportTitleGetSelectorTConcessionResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
