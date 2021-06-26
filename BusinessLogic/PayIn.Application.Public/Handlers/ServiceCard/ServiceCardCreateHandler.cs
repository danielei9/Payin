using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardCreateHandler : IServiceBaseHandler<ServiceCardCreateArguments>
    {
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCardBatch> ServiceCardBatchRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCardCreateArguments arguments)
		{
			var serviceCardBatch = (await ServiceCardBatchRepository.GetAsync())
				.Where(x =>
					x.Id == arguments.ServiceCardBatchId &&
					x.State == Common.ServiceCardBatchState.Active
				)
				.FirstOrDefault();
			if (serviceCardBatch == null)
				throw new ArgumentNullException("ServiceCardBatch");
			
			var cardUids = Regex.Split(arguments.Uids, "\\W+");
			foreach (var cardUid in cardUids)
			{
				var newServiceCard = new ServiceCard
				{
					Uid = ServiceCard.GetUidLong(cardUid, arguments.UidFormat),
					State = Common.ServiceCardState.Active,
					ConcessionId = serviceCardBatch.ConcessionId,
					SystemCardId = serviceCardBatch.SystemCardId,
					ServiceCardBatchId = serviceCardBatch.Id
				};
				newServiceCard.UidText = ServiceCard.GetUidText(newServiceCard.Uid,serviceCardBatch.UidFormat);
				await ServiceCardRepository.AddAsync(newServiceCard);
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}

