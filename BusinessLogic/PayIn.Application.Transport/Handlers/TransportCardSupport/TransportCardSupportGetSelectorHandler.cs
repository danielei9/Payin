using PayIn.Application.Dto.Transport.Arguments.TransportCardSupport;
using PayIn.Application.Dto.Transport.Results.TransportCardSupport;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportCardSupportGetSelectorHandler :
		IQueryBaseHandler<TransportCardSupportGetSelectorArguments, TransportCardSupportGetSelectorResult>
	{
		private readonly IEntityRepository<TransportCardSupport> Repository;

		#region Constructors
		public TransportCardSupportGetSelectorHandler(IEntityRepository<TransportCardSupport> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportCardSupportGetSelectorResult>> ExecuteAsync(TransportCardSupportGetSelectorArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.OwnerName.Contains(arguments.Filter))
				.GroupBy(y => y.OwnerName)
				.Select(z => z.FirstOrDefault())			
				.ToList()
				.Select (x => new TransportCardSupportGetSelectorResult
				 {
					Id = x.OwnerCode,
					Value = x.OwnerName
				 });

			return new ResultBase<TransportCardSupportGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
