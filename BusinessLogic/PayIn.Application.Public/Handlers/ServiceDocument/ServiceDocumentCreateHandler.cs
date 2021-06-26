using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceDocumentCreateHandler : IServiceBaseHandler<ServiceDocumentCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceDocument> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceDocumentCreateArguments arguments)
		{
			var item = new ServiceDocument
			{
				State = ServiceDocumentState.Active,
				Name = arguments.Name,
				Since = arguments.Since,
				Until = arguments.Until ?? XpDateTime.MaxValue,
				//Url = arguments.Url,
				IsVisible = true,
				Visibility = ServiceDocumentVisibility.Members,
				SystemCardId = arguments.SystemCardId
			};
			await Repository.AddAsync(item);

			//Save file in Azure
			var repositoryAzure = new AzureBlobRepository();
			var guid = Guid.NewGuid();
			item.Url = repositoryAzure.SaveFile(ServiceDocumentResources.ShortUrl.FormatString(item.Id, guid), arguments.Url);

			return item;
		}
		#endregion ExecuteAsync
	}
}
