using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Handlers
{
	public abstract class CreateHandler<TArguments, TEntity> : IServiceBaseHandler<TArguments>
		where TArguments : ICreateArgumentsBase<TEntity>
		where TEntity: IEntity, new()
	{
		private IEntityRepository<TEntity> Repository;

		private Action<TArguments, TEntity> Execution;

		#region Constructors
		public CreateHandler(
			IEntityRepository<TEntity> repository,
			Action<TArguments, TEntity> execution = null
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
			Execution = execution;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TArguments arguments)
		{
			var item = new TEntity();
			Execution(arguments, item);
			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
