using PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities;
using PayIn.Application.Dto.Transport.Results.TransportSimultaneousTitleCompatibilities;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportSimultaneousTitleCompatibilitiesGetSelectorHandler :
		IQueryBaseHandler<TransportSimultaneousTitleCompatibilitiesGetSelectorArguments, TransportSimultaneousTitleCompatibilitiesGetSelectorResult>
	{
		private readonly IEntityRepository<TransportTitle> Repository;

		#region Constructors
		public TransportSimultaneousTitleCompatibilitiesGetSelectorHandler(IEntityRepository<TransportTitle> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportSimultaneousTitleCompatibilitiesGetSelectorResult>> ExecuteAsync(TransportSimultaneousTitleCompatibilitiesGetSelectorArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.Name.Contains(arguments.Filter))		
				.ToList()
				.Select (x => new TransportSimultaneousTitleCompatibilitiesGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				 });

			return new ResultBase<TransportSimultaneousTitleCompatibilitiesGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
