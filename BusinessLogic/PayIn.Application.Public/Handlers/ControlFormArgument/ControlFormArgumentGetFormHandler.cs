using PayIn.Application.Dto.Arguments.ControlFormArgument;
using PayIn.Application.Dto.Results.ControlFormArgument;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Public;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormArgumentGetFormHandler :
		IQueryBaseHandler<ControlFormArgumentGetFormArguments, ControlFormArgumentGetFormResult>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ControlFormArgument> _Repository;

		#region Constructors
		public ControlFormArgumentGetFormHandler(
			IEntityRepository<ControlFormArgument> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormArgumentGetFormResult>> ExecuteAsync(ControlFormArgumentGetFormArguments arguments)
		{
			var formArguments = await _Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				formArguments = formArguments.Where(x => (
					x.Name.Contains(arguments.Filter) ||
					x.Observations.Contains(arguments.Filter)
				));

			var result = formArguments
				.Where(x => x.FormId == arguments.FormId)
				.OrderBy(x => x.Order)
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Type = x.Type,
					MinOptions = x.MinOptions,
					MaxOptions = x.MaxOptions,
					State = x.State,
					Order = x.Order,
					Required = x.Required
				})
				.ToList()
				.Select(x => new ControlFormArgumentGetFormResult
				{
					Id = x.Id,
					Name = x.Name,
					Type = x.Type,
					TypeAlias = x.Type.ToEnumAlias(),
					MinOptions = x.MinOptions,
					MaxOptions = x.MaxOptions,
					State = x.State,
					Order = x.Order,
					Required = x.Required
				});

			return new ResultBase<ControlFormArgumentGetFormResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
