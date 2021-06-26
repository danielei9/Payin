using PayIn.Application.Dto.Transport.Arguments.TransportCardSupport;
using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportCardSupportDeleteHandler : IServiceBaseHandler<TransportCardSupportDeleteArguments>
	{
		private readonly IEntityRepository<TransportCardSupport> Repository;

		#region Constructors
		public TransportCardSupportDeleteHandler(
			IEntityRepository<TransportCardSupport> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportCardSupportDeleteArguments>.ExecuteAsync(TransportCardSupportDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = TransportCardSupportState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}

