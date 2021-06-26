using System;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Handlers
{
	public class CreateImplicitHandler<TArguments, TEntity> : CreateHandler<TArguments, TEntity>
		where TArguments : ICreateArgumentsBase<TEntity>
		where TEntity: IEntity, new()
	{
		#region Constructors
		public CreateImplicitHandler(
			IEntityRepository<TEntity> repository
		)
			: base(repository,
			(arguments, entity) => {
				foreach (var property in typeof(TArguments).GetProperties())
				{
					var value = property.GetValue(arguments);
					entity.SetPropertyValue(property.Name, value);
				}
			})
		{
		}
		#endregion Constructors
	}
}
