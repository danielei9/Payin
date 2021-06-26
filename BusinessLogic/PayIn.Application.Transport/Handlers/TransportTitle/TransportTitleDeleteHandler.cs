using PayIn.Application.Dto.Transport.Arguments.TransportCardSupport;
using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
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
	class TransportTitleDeleteHandler : IServiceBaseHandler<TransportTitleDeleteArguments>
	{
		private readonly IEntityRepository<TransportTitle> Repository;

		#region Constructors
		public TransportTitleDeleteHandler(
			IEntityRepository<TransportTitle> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportTitleDeleteArguments>.ExecuteAsync(TransportTitleDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = TransportTitleState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}

