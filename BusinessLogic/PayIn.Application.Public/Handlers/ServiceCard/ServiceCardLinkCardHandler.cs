using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
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
	public class ServiceCardLinkCardHandler : IServiceBaseHandler<ServiceCardLinkCardArguments>
    {
		[Dependency] public MobileServiceCardLinkHandler VinculateCard { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> serviceCardRepository { get; set; }
		//[Dependency] public IEntityRepository<ServiceCardBatch> ServiceCardBatchRepository { get; set; }

		private string hexArray = "0123456789ABCDEF";

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCardLinkCardArguments arguments)
		{
			var card = (await serviceCardRepository.GetAsync(nameof(ServiceCard.LinkedUsers)))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();
			if (card == null)
				throw new ArgumentNullException("cardId");

			var vinculateLoginCard = (await serviceCardRepository.GetAsync())
				.OrderByDescending(x => x.Id)
				.Where(x => x.UidText == arguments.UidText)
				.Select(x => x.OwnerUser.Login ?? "")
				.FirstOrDefault();
			if (vinculateLoginCard == "")
				throw new ArgumentNullException("UidText");

			//if (card.ConcessionId != vinculateCard.ConcessionId)
			//	throw new ArgumentException("Las tarjetas deben pertenecer al mismo sistema");

			// Link cards
			card.LinkedUsers.Add(new ServiceUserLink { Login = vinculateLoginCard });

			return card;
		}
		#endregion ExecuteAsync
	}
}

