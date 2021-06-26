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
	public class TransportTitleGetSelectorHandler :
		IQueryBaseHandler<TransportTitleGetSelectorArguments, TransportTitleGetSelectorResult>
	{
		private readonly IEntityRepository<TransportTitle> Repository;

		#region Constructors
		public TransportTitleGetSelectorHandler(IEntityRepository<TransportTitle> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportTitleGetSelectorResult>> ExecuteAsync(TransportTitleGetSelectorArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.OwnerName.Contains(arguments.Filter))
				.GroupBy(y => y.OwnerName)
				.Select(z => z.FirstOrDefault())			
				.ToList()
				.Select (x => new TransportTitleGetSelectorResult
				 {
					Id = x.OwnerCode,
					Value = x.OwnerName
				 });

			return new ResultBase<TransportTitleGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
