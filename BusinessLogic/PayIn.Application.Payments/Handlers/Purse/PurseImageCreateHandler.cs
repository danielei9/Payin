using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	public class PurseImageCreateHandler :
		IServiceBaseHandler<PurseImageCreateArguments>
	{
		private readonly IEntityRepository<Purse> Repository;

		#region Constructors
		public PurseImageCreateHandler(
			IEntityRepository<Purse> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PurseImageCreateArguments arguments)
		{
			var AzureRepos = new AzureBlobRepository();
			var item = await Repository.GetAsync(arguments.PurseId);

			#if TEST || DEBUG || EMULATOR
			item.Image = PurseResources.FotoUrlTest.FormatString(arguments.PurseId);
			AzureRepos.SaveImage(PurseResources.FotoShortUrlTest.FormatString(arguments.PurseId), arguments.Image);
			#endif // TEST || DEBUG
				item.Image = PurseResources.FotoUrl.FormatString(arguments.PurseId);
				AzureRepos.SaveImage(PurseResources.FotoShortUrl.FormatString(arguments.PurseId), arguments.Image);

			return item;
		}
		#endregion ExecuteAsync
	}
}
