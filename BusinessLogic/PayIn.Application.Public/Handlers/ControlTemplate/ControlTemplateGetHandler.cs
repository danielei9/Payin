using PayIn.Application.Dto.Arguments.ControlTemplate;
using PayIn.Application.Dto.Results.ControlTemplate;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateGetHandler :
		IQueryBaseHandler<ControlTemplateGetArguments, ControlTemplateGetResult>
	{
		private readonly IEntityRepository<ControlTemplate> Repository;

		#region Constructors
		public ControlTemplateGetHandler(IEntityRepository<ControlTemplate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlTemplateGetResult>> IQueryBaseHandler<ControlTemplateGetArguments, ControlTemplateGetResult>.ExecuteAsync(ControlTemplateGetArguments arguments)
		{
			var items = await Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					CheckDuration = x.CheckDuration,
					Monday = x.Monday,
					Tuesday = x.Tuesday,
					Wednesday = x.Wednesday,
					Thursday = x.Thursday,
					Friday = x.Friday,
					Saturday = x.Saturday,
					Sunday = x.Sunday
				})
				.ToList()
				.Select(x => new ControlTemplateGetResult
				{
					Id = x.Id,
					Name = x.Name ?? "",
					Observations = x.Observations ?? "",
					CheckDuration = x.CheckDuration, // Need to be done in memory
					Monday = x.Monday,
					Tuesday = x.Tuesday,
					Wednesday = x.Wednesday,
					Thursday = x.Thursday,
					Friday = x.Friday,
					Saturday = x.Saturday,
					Sunday = x.Sunday
				});

			return new ResultBase<ControlTemplateGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
