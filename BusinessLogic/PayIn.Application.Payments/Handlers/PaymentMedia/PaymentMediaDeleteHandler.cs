using PayIn.Application.Dto.Arguments.PaymentMedia;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentMediaDeleteHandler :
			IServiceBaseHandler<PaymentMediaDeleteArguments>
	{
		private readonly IEntityRepository<PaymentMedia> Repository;

		#region Constructors
		public PaymentMediaDeleteHandler(IEntityRepository<PaymentMedia> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region PaymentMediaDelete
		async Task<dynamic> IServiceBaseHandler<PaymentMediaDeleteArguments>.ExecuteAsync(PaymentMediaDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion PaymentMediaDelete
	}
}
