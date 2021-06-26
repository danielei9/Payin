using PayIn.Application.Dto.Arguments.PaymentMedia;
using PayIn.Domain.Internal;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	public class PaymentMediaCreateHandler :
		IServiceBaseHandler<PaymentMediaCreateArguments>
	{
		public readonly IEntityRepository<PaymentMedia> Repository;

		#region Contructors
		public PaymentMediaCreateHandler(
			IEntityRepository<PaymentMedia> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PaymentMediaCreateArguments>.ExecuteAsync(PaymentMediaCreateArguments command)
		{
			var item = new PaymentMedia
			{
				Name = command.Name,
				Type = command.Type,
				Number = command.Number,
				ExpirationMonth = command.ExpirationMonth,
				ExpirationYear = command.ExpirationYear,
				PublicId = command.Id,
				//Login = SessionData.Login
			};
			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
