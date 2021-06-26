using PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility;
using PayIn.Application.Dto.Transport.Results.TransportCardSupportTitleCompatibility;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportCardSupportTitleCompatibilityGetSelectorHandler :
		IQueryBaseHandler<TransportCardSupportTitleCompatibilityGetSelectorArguments, TransportCardSupportTitleCompatibilityGetSelectorResult>
	{
		private readonly IEntityRepository<TransportCardSupport> Repository;

		#region Constructors
		public TransportCardSupportTitleCompatibilityGetSelectorHandler(IEntityRepository<TransportCardSupport> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportCardSupportTitleCompatibilityGetSelectorResult>> ExecuteAsync(TransportCardSupportTitleCompatibilityGetSelectorArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.Name.Contains(arguments.Filter))		
				.ToList()
				.Select (x => new TransportCardSupportTitleCompatibilityGetSelectorResult
                {
					Id = x.Id,
					Value = x.Name
				 });

			return new ResultBase<TransportCardSupportTitleCompatibilityGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
