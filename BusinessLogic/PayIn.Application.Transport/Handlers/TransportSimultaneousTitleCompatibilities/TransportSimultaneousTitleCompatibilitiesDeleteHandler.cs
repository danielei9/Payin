using PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportSimultaneousTitleCompatibilitiesDeleteHandler : IServiceBaseHandler<TransportSimultaneousTitleCompatibilitiesDeleteArguments>
	{
		private readonly IEntityRepository<TransportSimultaneousTitleCompatibility> Repository;

		#region Constructors
		public TransportSimultaneousTitleCompatibilitiesDeleteHandler(
			IEntityRepository<TransportSimultaneousTitleCompatibility> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportSimultaneousTitleCompatibilitiesDeleteArguments>.ExecuteAsync(TransportSimultaneousTitleCompatibilitiesDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

            await Repository.DeleteAsync(item);
            return null;
		}
		#endregion ExecuteAsync
	}
}

