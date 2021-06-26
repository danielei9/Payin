using System;
using System.Linq;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Handlers
{
	public class UpdateImplicitHandler<TArguments, TEntity> : UpdateHandler<TArguments, TEntity>
		where TArguments : IUpdateArgumentsBase<TEntity>
		where TEntity: IEntity
	{
		private IEntityRepository<TEntity> Repository;

		#region Constructors
		public UpdateImplicitHandler(
			IEntityRepository<TEntity> repository
		)
			: base(repository,
			(arguments, entity) =>
			{
				foreach (var property in typeof(TArguments).GetProperties().Where(x => x.Name != "Id"))
				{
					var value = property.GetValue(arguments);
					entity.SetPropertyValue(property.Name, value);
				}
			})
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors
	}
}
