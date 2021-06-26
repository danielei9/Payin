using PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Microsoft.Practices.Unity;
using System.Linq;


namespace PayIn.Application.Transport.Handlers
{
	public class TransportCardSupportTitleCompatibilityUpdateHandler :
		IServiceBaseHandler<TransportCardSupportTitleCompatibilityUpdateArguments>
	{
		[Dependency] public IEntityRepository<TransportCardSupport> RepositoryTransportCard { get; set; }
		[Dependency] public IEntityRepository<TransportCardSupportTitleCompatibility> Repository { get; set; }
		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportCardSupportTitleCompatibilityUpdateArguments>.ExecuteAsync(TransportCardSupportTitleCompatibilityUpdateArguments arguments)
		{
			var id = (await RepositoryTransportCard.GetAsync())
			   .Where(x => x.Name.Equals(arguments.TransportCardSupportName))
			   .Select(x => x.Id)
			   .FirstOrDefault();
			   
			if (id.Equals(0))
				throw new ArgumentNullException("id");

			var item = await Repository.GetAsync(arguments.Id);
			item.TransportTitleId = arguments.TransportTitleId;
            item.TransportCardSupportId = id;
            
			return item;
		}
		#endregion ExecuteAsync
	}
}
