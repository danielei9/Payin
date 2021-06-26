using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.Application.Dto.Payments.Results.Promotion;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Promotions;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionGetAllConcessionHandler :
		IQueryBaseHandler<PromotionGetAllConcessionArguments, PromotionGetAllConcessionResult>
	{
		private readonly IEntityRepository<Promotion> Repository;
		private readonly IEntityRepository<PromoExecution> ExecutionRepository;
		private readonly IEntityRepository<TransportConcession> ConcessionRepository;
		private readonly IEntityRepository<TransportTitle> TitleRepository;
		private readonly IEntityRepository<TransportOperation> OperationRepository;
		private readonly ISessionData SessionData;

		#region Constructor
		public PromotionGetAllConcessionHandler(
			IEntityRepository<Promotion> repository,
			IEntityRepository<PromoExecution> executionRepository,
			IEntityRepository<TransportConcession> concessionRepository,
			IEntityRepository<TransportTitle> titleRepository,
			IEntityRepository<TransportOperation> operationRepository,
			ISessionData sessionData
		)
		{
			if (repository == null) throw new ArgumentNullException("Promotion");
			if (executionRepository == null) throw new ArgumentNullException("ExecutionRepository");
			if(concessionRepository == null) throw new ArgumentNullException("ConcessionRepository");
			if (titleRepository == null) throw new ArgumentNullException("titleRepository");
			if(operationRepository == null) throw new ArgumentNullException("operationRepository");
			if (sessionData == null) throw new ArgumentNullException("SessionData");

			Repository = repository;
			SessionData = sessionData;
			ConcessionRepository = concessionRepository;
			ExecutionRepository = executionRepository;
			TitleRepository = titleRepository;
			OperationRepository = operationRepository;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<PromotionGetAllConcessionResult>> ExecuteAsync(PromotionGetAllConcessionArguments arguments)
		{
			var concession = (await ConcessionRepository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Concession.Concession.Supplier.Login == SessionData.Login)
				.FirstOrDefault();

			var titles = (await TitleRepository.GetAsync())
				.Where(x => x.TransportConcessionId == concession.Id && x.OperateByPayIn == true && x.State == TransportTitleState.Active)
				.ToList();

			var executions = (await ExecutionRepository.GetAsync("Promotion"))
				.Where(x => x.PromoUserId != null && x.TransportOperationId != null)
				.ToList();

			List<Promotion> promotions = new List<Promotion>();
			foreach (var execution in executions)
			{
				var operation = (await OperationRepository.GetAsync())
					.Where(x => x.Id == execution.TransportOperationId && x.TransportPriceId != null)
					;
				foreach (var title in titles)
				{
					title.Prices.Where(y => y.Id == operation.Select(z=> z.Id).FirstOrDefault());
					if (title != null && promotions.Find (x=> x.Id == execution.PromotionId) == null)
					{
						promotions.Add(execution.Promotion);
					}
				}
				
			}
			var result = promotions
				.Select(x => new
				{
					Id = x.Id,
				    Name = x.Name,
					StartDate = x.StartDate,
					EndDate = (x.EndDate == XpDateTime.MaxValue) ? null : (DateTime?) x.EndDate,
					Acumulative = x.Acumulative,
					State = x.State,
					ConcessionName = concession.Concession.Concession.Name,
					CodeApplied = x.PromoExecutions.Where(y => y.AppliedDate != null).Count(),
					TotalCode = x.PromoExecutions.Count(),
					TotalCost = x.PromoExecutions.Sum( z=> z.Cost)
				})
				.ToList()
				.OrderByDescending(x => x.StartDate)
				.Select(x => new PromotionGetAllConcessionResult
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

			return new ResultBase<PromotionGetAllConcessionResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
