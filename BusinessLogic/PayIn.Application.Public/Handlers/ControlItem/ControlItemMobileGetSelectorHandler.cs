using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Application.Dto.Results.ControlItem;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlItemMobileGetSelectorHandler :
		IQueryBaseHandler<ControlItemMobileGetSelectorArguments, ControlItemMobileGetSelectorResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ControlItem> RepositoryControlItem;
		private readonly IEntityRepository<ServiceWorker> RepositoryServiceWorker;
		private readonly IEntityRepository<ServiceConcession> RepositoryServiceConcession;

		#region Constructors
		public ControlItemMobileGetSelectorHandler(
			ISessionData sessionData,
			IEntityRepository<ControlItem> repositoryControlItem,
			IEntityRepository<ServiceWorker> repositoryServiceWorker,
			IEntityRepository<ServiceConcession> repositoryServiceConcession)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repositoryControlItem == null) throw new ArgumentNullException("repositoryControlItem");
			if (repositoryServiceWorker == null) throw new ArgumentNullException("repositoryServiceWorker");
			if (repositoryServiceConcession == null) throw new ArgumentNullException("repositoryServiceConcession");

			SessionData = sessionData;
			RepositoryControlItem = repositoryControlItem;
			RepositoryServiceWorker = repositoryServiceWorker;
			RepositoryServiceConcession = repositoryServiceConcession;
		}
		#endregion Constructor

		#region ExecuteAsync
		async Task<ResultBase<ControlItemMobileGetSelectorResult>> IQueryBaseHandler<ControlItemMobileGetSelectorArguments, ControlItemMobileGetSelectorResult>.ExecuteAsync(ControlItemMobileGetSelectorArguments arguments)
		{
			var now = DateTime.Now;
			var yesterday = now.AddDays(-1);

			var result = (await RepositoryControlItem.GetAsync())
				.Where(x => x.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login))
				.Select(x => new ControlItemMobileGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ControlItemMobileGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
