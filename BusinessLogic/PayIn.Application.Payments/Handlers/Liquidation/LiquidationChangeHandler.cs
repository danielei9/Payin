using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Liquidation", "Change")]
	public class LiquidationChangeHandler :
		IServiceBaseHandler<LiquidationChangeArguments>
	{
		private readonly IEntityRepository<PaymentConcession> ConcessionRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public LiquidationChangeHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentConcession> concessionRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;

			if (concessionRepository == null)
			throw new ArgumentNullException("concessionRepository");
			ConcessionRepository = concessionRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(LiquidationChangeArguments arguments)
		{
			var now = DateTime.Now;

			var concession = (await ConcessionRepository.GetAsync("Concession"))
				.Where(x => x.Concession.Supplier.Login == SessionData.Login)
				.FirstOrDefault();
			if (concession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(LiquidationResources.ConfirmChangeNonActiveException, "login");

			concession.LiquidationRequestDate = now.ToUTC();
			return concession;
		}
		#endregion ExecuteAsync
	}
}
