using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Payments.PaymentUser;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentUserUpdateHandler :
		IServiceBaseHandler<PaymentUserUpdateArguments>
	{
		private readonly IEntityRepository<serV> Repository;

		#region Constructors
		public PaymentUserUpdateHandler(IEntityRepository<serV> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PaymentUserUpdateArguments>.ExecuteAsync(PaymentUserUpdateArguments arguments)
		{
			var paymentUserItem = await Repository.GetAsync(arguments.Id);
			paymentUserItem.Name = arguments.Name;

			return paymentUserItem;
		}
		#endregion ExecuteAsync
	}
}
