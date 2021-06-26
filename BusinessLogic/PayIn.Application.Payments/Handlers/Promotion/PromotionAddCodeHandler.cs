using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common.Resources;
using PayIn.Common;
using Xp.Common.Exceptions;
using System.Collections.Generic;
using Xp.Common;
using PayIn.Domain.Payments;
using PayIn.Application.Payments.Services;
using PayIn.Domain.Promotions;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionAddCodeHandler :
		IServiceBaseHandler<PromotionAddCodeArguments>
	{
		private readonly PromotionService PromotionService;		
		private readonly IEntityRepository<PromoExecution> ExecutionRepository;	

		#region Constructors
		public PromotionAddCodeHandler(
			PromotionService promotionService,			
			IEntityRepository<PromoExecution> executionRepository
		)
		{			
			if (promotionService == null) throw new ArgumentNullException("promotionService");			
			if (executionRepository == null) throw new ArgumentNullException("executionRepository");
		
			PromotionService = promotionService;
			ExecutionRepository = executionRepository;		
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PromotionAddCodeArguments arguments)
		{

			if(arguments.Quantity > 999999)			
				throw new ApplicationException(PromotionResources.MaxCodeQuantityException);

			var now = DateTime.Now;
			var lastNumber = (await ExecutionRepository.GetAsync())
				.Where(x => x.PromotionId == arguments.Id)
				.OrderByDescending(x => x.Number)
				.FirstOrDefault();

			for (int i = 1; i < arguments.Quantity; i++)
			{
				var code =  PromotionService.CreateCode(PromotionExecutionType.AlphaNumeric, arguments.Id, lastNumber.Number + i);

				var execution = new PromoExecution
				{				
					Code = code,
					PromotionId = arguments.Id,
					Type = PromotionExecutionType.AlphaNumeric,
					Number = lastNumber.Number + i
				};
				await ExecutionRepository.AddAsync(execution);
			}

			return 1;
		}
		#endregion ExecuteAsync
	}
}

