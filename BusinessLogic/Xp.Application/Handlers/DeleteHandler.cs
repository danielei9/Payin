using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Handlers
{
	public abstract class DeleteHandler<TArguments, TEntity> : IServiceBaseHandler<TArguments>
		where TArguments : IDeleteArgumentsBase<TEntity>
		where TEntity: IEntity
	{
		private IEntityRepository<TEntity> Repository;

		private Action<TArguments, TEntity> Execution;

		#region Constructors
		public DeleteHandler(
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
			var item = await Repository.GetAsync(arguments.Id);
			if (Execution != null)
				Execution(arguments, item);
			await Repository.DeleteAsync(item);
			
			return item;
		}
		#endregion ExecuteAsync
	}
}
