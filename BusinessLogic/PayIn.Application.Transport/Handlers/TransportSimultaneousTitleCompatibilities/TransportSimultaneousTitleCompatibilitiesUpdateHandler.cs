using PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Microsoft.Practices.Unity;
using System.Linq;


namespace PayIn.Application.Transport.Handlers
{
	public class TransportSimultaneousTitleCompatibilitiesUpdateHandler :
		IServiceBaseHandler<TransportSimultaneousTitleCompatibilitiesUpdateArguments>
	{
		[Dependency] public IEntityRepository<TransportTitle> RepositoryTransportTitle { get; set; }
		[Dependency] public IEntityRepository<TransportSimultaneousTitleCompatibility> Repository { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportSimultaneousTitleCompatibilitiesUpdateArguments>.ExecuteAsync(TransportSimultaneousTitleCompatibilitiesUpdateArguments arguments)
		{
			var id = (await RepositoryTransportTitle.GetAsync())
			   .Where(x => x.Name.Equals(arguments.Name))
			   .Select(x => x.Id)
			   .FirstOrDefault();

			if (id.Equals(0))
				throw new ArgumentNullException("id");

			var item = await Repository.GetAsync(arguments.Id);
			item.TransportTitleId = arguments.TransportTitleId;
            item.TransportTitle2Id = id;

			return item;
		}
		#endregion ExecuteAsync
	}
}
