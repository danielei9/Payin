using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Payments.Purse;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Payments;
using PayIn.Common.Resources;
using System.Collections.Generic;
using Xp.Common;
using PayIn.Common;
using PayIn.BusinessLogic.Common;
using PayIn.Application.Dto.Arguments.Notification;

namespace PayIn.Application.Public.Handlers
{
	public class PurseUpdateHandler :
		IServiceBaseHandler<PurseUpdateArguments>
	{
		private readonly IEntityRepository<serV> Repository;

		#region Constructors
		public PurseUpdateHandler(
			IEntityRepository<serV> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PurseUpdateArguments>.ExecuteAsync(PurseUpdateArguments arguments)
		{
			var now = DateTime.Now;
			var item = (await Repository.GetAsync("PaymentMedias"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			if ((arguments.Validity < now) || (arguments.Validity < item.Validity))
				throw new Exception(PurseResources.LowerValidityException);

			if ((arguments.Expiration < now) || (arguments.Expiration < item.Expiration))
				throw new Exception(PurseResources.LowerExpirationException);

			item.Name = arguments.Name;
			item.Expiration = arguments.Expiration;
			item.Validity = arguments.Validity;

			foreach (var paymentmedia in item.PaymentMedias) {
				paymentmedia.Name = arguments.Name;
			}

            return item;
		}
		#endregion item
	}
}
