using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentUserMobileBlockConcessionHandler : IServiceBaseHandler<PaymentUserMobileBlockConcessionArguments>
	{
		private readonly IEntityRepository<PaymentUser> Repository;

		#region Constructors
		public PaymentUserMobileBlockConcessionHandler(
			IEntityRepository<PaymentUser> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentUserMobileBlockConcessionArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = PaymentUserState.Blocked;
			return item;
		}
		#endregion ExecuteAsync
	}
}