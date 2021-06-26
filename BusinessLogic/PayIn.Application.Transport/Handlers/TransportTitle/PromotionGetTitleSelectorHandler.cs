using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
using PayIn.Application.Dto.Transport.Results.TransportTitle;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PromotionGetTitleSelectorHandler :
		IQueryBaseHandler<PromotionGetTitleSelectorArguments, PromotionGetTitleSelectorResult>
	{
		private readonly IEntityRepository<TransportPrice> Repository;

		#region Constructors
		public PromotionGetTitleSelectorHandler(IEntityRepository<TransportPrice> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PromotionGetTitleSelectorResult>> ExecuteAsync(PromotionGetTitleSelectorArguments arguments)
		{
			var items = await Repository.GetAsync("Title");

			var result = items
				.Where(x => x.Title.Name.Contains(arguments.Filter) && x.Title.OperateByPayIn == true)
				;
			var res = result	
				.Where(x=> x.Version == result.Max(y => y.Version))
				.ToList()
				.Select(x => new PromotionGetTitleSelectorResult
				{
					Id = x.Id,
					Value = x.Title.Name + " " + x.Zone.ToEnumAlias() + " (" + x.Price + "€)"					 
				})
				;

			return new ResultBase<PromotionGetTitleSelectorResult> { Data = res };
		}
		#endregion ExecuteAsync
	}
}
