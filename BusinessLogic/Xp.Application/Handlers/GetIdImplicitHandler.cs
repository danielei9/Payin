using System;
using System.Collections.Generic;
using System.Linq;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Handlers
{
	public class GetIdImplicitHandler<TArguments, TResult, TEntity> : GetHandler<TArguments, TResult, TEntity>
		where TArguments : IGetIdArgumentsBase<TResult, TEntity>
		where TEntity: IEntity
	{
		#region Constructors
		public GetIdImplicitHandler(
			IEntityRepository<TEntity> repository
		)
			: base(
				repository,
				(arguments, entities) => {
					var properties = typeof(TResult).GetProperties();

					var l = entities.ToList().Cast<TEntity>();
					var l2 = entities
						.Cast<TEntity>()
						.Where(x => x.Id == 3).ToList();

					var result = new List<TResult>();
					foreach (var item in entities.Where(x => x.Id == arguments.Id))
					{
						var temp = Activator.CreateInstance<TResult>();
						result.Add(temp);

						foreach (var property in properties)
						{
							var value = property.GetValue(item);
							temp.SetPropertyValue(property.Name, value);
						}
					}

					return result;
				}
			)
		{
		}
		#endregion Constructors
	}
}
