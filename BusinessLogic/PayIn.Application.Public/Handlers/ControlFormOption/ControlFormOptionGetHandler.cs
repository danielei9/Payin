using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormOptionGetHandler :
		IQueryBaseHandler<ControlFormOptionGetArguments, ControlFormOptionGetResult>
	{
		private readonly IEntityRepository<ControlFormOption> Repository;

		#region Constructors
		public ControlFormOptionGetHandler(
			IEntityRepository<ControlFormOption> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormOptionGetResult>> ExecuteAsync(ControlFormOptionGetArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = item
				.Select(x => new
				{
					Id			=	x.Id,
					Text		=	x.Text,
					Value		=	x.Value,
					ArgumentId	=	x.ArgumentId
				})
				.Select(x => new ControlFormOptionGetResult
				{
					Id			=	x.Id,
					Text		=	x.Text,
					Value		=	x.Value,
					ArgumentId	=	x.ArgumentId
				});
			return new ResultBase<ControlFormOptionGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
