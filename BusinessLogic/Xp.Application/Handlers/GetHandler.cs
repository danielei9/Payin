using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Handlers
{
	public abstract class GetHandler<TArguments, TResult, TEntity> : IQueryBaseHandler<TArguments, TResult>
		where TArguments : IGetArgumentsBase<TEntity>
		where TEntity: IEntity
	{
		private IEntityRepository<TEntity> Repository;

		private Func<TArguments, IQueryable<TEntity>, IEnumerable<TResult>> Execution;

		#region Constructors
		public GetHandler(
			IEntityRepository<TEntity> repository,
			Func<TArguments, IQueryable<TEntity>, IEnumerable<TResult>> execution
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (execution == null) throw new ArgumentNullException("execution");

			Repository = repository;
			Execution = execution;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TResult>> ExecuteAsync(TArguments arguments)
		{
			var items = await Repository.GetAsync();
			var result = Execution(arguments, items);
			return new ResultBase<TResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
