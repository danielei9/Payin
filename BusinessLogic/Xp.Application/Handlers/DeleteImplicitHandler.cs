using System;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Handlers
{
	public class DeleteImplicitHandler<TArguments, TEntity> : DeleteHandler<TArguments, TEntity>
		where TArguments : IDeleteArgumentsBase<TEntity>
		where TEntity: IEntity
	{
		private IEntityRepository<TEntity> Repository;

		#region Constructors
		public DeleteImplicitHandler(
			IEntityRepository<TEntity> repository
		)
			: base(repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors
	}
}
