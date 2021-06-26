using PayIn.Application.Dto.Arguments.ControlFormArgument;
using PayIn.Application.Dto.Results.ControlFormArgument;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormArgumentGetHandler :
		IQueryBaseHandler<ControlFormArgumentGetArguments, ControlFormArgumentGetResult>
	{
		private readonly IEntityRepository<ControlFormArgument> _Repository;

		#region Constructors
		public ControlFormArgumentGetHandler(
			IEntityRepository<ControlFormArgument> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlFormArgumentGetResult>> IQueryBaseHandler<ControlFormArgumentGetArguments, ControlFormArgumentGetResult>.ExecuteAsync(ControlFormArgumentGetArguments arguments)
		{
			var formArguments = await _Repository.GetAsync();

				var result = formArguments
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					Type = x.Type,
					MinOptions = x.MinOptions,
					MaxOptions = x.MaxOptions,
					Order = x.Order,
					Required = x.Required
				})
				.ToList()
				.Select(x => new ControlFormArgumentGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					Type = x.Type,
					MinOptions = x.MinOptions,
					MaxOptions = x.MaxOptions,
					Required = x.Required,
					Order = x.Order

				})
				.OrderBy(x => x.Id);
				return new ResultBase<ControlFormArgumentGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
