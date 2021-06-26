using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.Application.Dto.Payments.Results.Promotion;
using PayIn.Common;
using PayIn.Domain.Promotions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionGetAllHandler :
		IQueryBaseHandler<PromotionGetAllArguments, PromotionGetAllResult>
	{
		private readonly IEntityRepository<Promotion> Repository;		

		#region Constructor
		public PromotionGetAllHandler(
			IEntityRepository<Promotion> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("GreyList");
			Repository = repository;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<PromotionGetAllResult>> ExecuteAsync(PromotionGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync("Concession.Concession"))
				.Where(x => x.State == PromotionState.Active);

			var result = items
				.Select(x => new
				{
					Id = x.Id,
				    Name = x.Name,
					StartDate = x.StartDate,
					EndDate = (x.EndDate == XpDateTime.MaxValue) ? null : (DateTime?) x.EndDate,
					Acumulative = x.Acumulative,
					State = x.State,
					ConcessionName = x.Concession.Concession.Name,
					CodeApplied = x.PromoExecutions.Where(y => y.AppliedDate != null).Count(),
					TotalCode = x.PromoExecutions.Any() ? x.PromoExecutions.Count() : 0,
					TotalCost = x.PromoExecutions.Any() ? x.PromoExecutions.Sum( z=> z.Cost) : 0
				})
				.ToList()
				.OrderByDescending(x => x.StartDate)
				.Select(x => new PromotionGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					StartDate = x.StartDate,
					EndDate = x.EndDate,
					Acumulative = x.Acumulative,
					State = x.State,
					ConcessionName = x.ConcessionName,
					CodeApplied = x.CodeApplied,
					TotalCode = x.TotalCode,
					TotalCost = x.TotalCost
				});

			return new ResultBase<PromotionGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
