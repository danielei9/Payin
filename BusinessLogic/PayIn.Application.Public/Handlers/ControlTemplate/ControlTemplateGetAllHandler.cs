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
	public class ControlTemplateGetAllHandler :
		IQueryBaseHandler<ControlTemplateGetAllArguments, ControlTemplateGetAllResult>
	{
		private readonly IEntityRepository<ControlTemplate> Repository;

		#region Constructors
		public ControlTemplateGetAllHandler(IEntityRepository<ControlTemplate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlTemplateGetAllResult>> IQueryBaseHandler<ControlTemplateGetAllArguments, ControlTemplateGetAllResult>.ExecuteAsync(ControlTemplateGetAllArguments arguments)
		{
			var items = await Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x => 
						x.Name.Contains(arguments.Filter) ||
						x.Observations.Contains(arguments.Filter) ||
						x.Item.Name.Contains(arguments.Filter)
					);

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name ?? "",
					CheckDuration = x.CheckDuration,
					Monday = x.Monday,
					Tuesday = x.Tuesday,
					Wednesday = x.Wednesday,
					Thursday = x.Thursday,
					Friday = x.Friday,
					Saturday = x.Saturday,
					Sunday = x.Sunday,
					ItemId = x.ItemId,
					ItemName = x.Item.Name,
					TemplateItemsCount = x.TemplateItems
						.Count(),
					TemplateChecksCount = x.Checks
						.Where(y => y.ItemsSince.Count() + y.ItemsUntil.Count() == 0)
						.Count(),
					Items = (
						x.TemplateItems.Select(y => new	{	
							Type = ControlTemplateGetAllResult_Types.TemplateItem,
							Id = y.Id,
							Since = y.Since.Time,
							Until = (DateTime?) y.Until.Time
						})
					)
					.Union(
						x.Checks
							.Where(y => y.ItemsSince.Count() + y.ItemsUntil.Count() == 0)
							.Select(y => new {	
								Type = ControlTemplateGetAllResult_Types.TemplateCheck,
								Id = y.Id,
								Since = y.Time,
								Until = (DateTime?) null
							})
					)
				})
				.ToList()
				.Select(x => new ControlTemplateGetAllResult
				{
					Id = x.Id,
					Name = x.Name ?? "",
					CheckDuration = x.CheckDuration, // Need to be done in memory
					Monday = x.Monday,
					Tuesday = x.Tuesday,
					Wednesday = x.Wednesday,
					Thursday = x.Thursday,
					Friday = x.Friday,
					Saturday = x.Saturday,
					Sunday = x.Sunday,
					ItemId = x.ItemId,
					ItemName = x.ItemName,
					TemplateItemsCount = x.TemplateItemsCount,
					TemplateChecksCount = x.TemplateChecksCount,
					Items = x.Items
						.Select(y =>
							new ControlTemplateGetAllResult_Item()
							{	
								Id = y.Id,
								Since = y.Since, // Need to be done in memory
								Until = y.Until // Need to be done in memory
							}
						)
				});

			return new ResultBase<ControlTemplateGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
