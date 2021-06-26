using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCard;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class SystemCardUpdateHandler : IServiceBaseHandler<SystemCardUpdateArguments>
	{
		[Dependency] public IEntityRepository<SystemCard> RepositorySystemCard { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SystemCardUpdateArguments arguments)
		{
			var systemCard = await RepositorySystemCard.GetAsync(arguments.Id);

			systemCard.Name = arguments.Name;
			systemCard.CardType = arguments.CardType;
			systemCard.NumberGenerationType = arguments.NumberGenerationType;
			systemCard.PhotoUrl = arguments.PhotoUrl ?? "";
			systemCard.AffiliationEmailBody = arguments.AffiliationEmailBody ?? "";
			systemCard.SynchronizationInterval = arguments.SynchronizationInterval;
			systemCard.ClientId = arguments.ClientId;
			if (arguments.ConcessionOwnerId > 0)
				systemCard.ConcessionOwnerId = arguments.ConcessionOwnerId;

			return systemCard;
		}
		#endregion ExecuteAsync
	}
}
