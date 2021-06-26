using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentUserMobileUnblockConcessionHandler : IServiceBaseHandler<PaymentUserMobileUnblockConcessionArguments>
	{
		private readonly IEntityRepository<PaymentUser> Repository;

		#region Constructors
		public PaymentUserMobileUnblockConcessionHandler(
			IEntityRepository<PaymentUser> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentUserMobileUnblockConcessionArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = PaymentUserState.Pending;
			return item;
		}
		#endregion ExecuteAsync
	}
}