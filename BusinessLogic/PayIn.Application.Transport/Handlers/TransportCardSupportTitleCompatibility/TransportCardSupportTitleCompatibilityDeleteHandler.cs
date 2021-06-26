using PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportCardSupportTitleCompatibilityDeleteHandler : IServiceBaseHandler<TransportCardSupportTitleCompatibilityDeleteArguments>
	{
		private readonly IEntityRepository<TransportCardSupportTitleCompatibility> Repository;

		#region Constructors
		public TransportCardSupportTitleCompatibilityDeleteHandler(
			IEntityRepository<TransportCardSupportTitleCompatibility> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportCardSupportTitleCompatibilityDeleteArguments>.ExecuteAsync(TransportCardSupportTitleCompatibilityDeleteArguments arguments)
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

