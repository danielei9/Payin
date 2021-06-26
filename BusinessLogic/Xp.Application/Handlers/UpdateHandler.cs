using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace Xp.Application.Handlers
{
	public abstract class UpdateHandler<TArguments, TEntity> : IServiceBaseHandler<TArguments>
		where TArguments : IUpdateArgumentsBase<TEntity>
		where TEntity: IEntity
	{
		private IEntityRepository<TEntity> Repository;
		private Action<TArguments, TEntity> Execution;

		#region Constructors
		public UpdateHandler(
			IEntityRepository<TEntity> repository,
			Action<TArguments, TEntity> execution
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (execution == null) throw new ArgumentNullException("execution");

			Repository = repository;
			Execution = execution;
		}
		public UpdateHandler(
			IEntityRepository<TEntity> repository
		)
			: this(repository, (arguments, item) =>
			{
				foreach(var property in arguments.GetType()
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => x.Name.ToLower() != "id")
				)
					item.SetPropertyValue(property.Name, property.GetValue(arguments));
			})
		{
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			Execution(arguments, item);
			return item;
		}
		#endregion ExecuteAsync
	}
}
