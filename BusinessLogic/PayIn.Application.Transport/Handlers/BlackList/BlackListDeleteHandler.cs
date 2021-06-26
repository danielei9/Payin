using PayIn.Application.Dto.Transport.Arguments.BlackList;
using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using static PayIn.Domain.Transport.BlackList;

namespace PayIn.Application.Transport.Handlers
{
	public class BlackListDeleteHandler :
		IServiceBaseHandler<BlackListDeleteArguments>
	{
		private readonly IEntityRepository<BlackList> Repository;

		#region Constructor
		public BlackListDeleteHandler(
			IEntityRepository<BlackList> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(BlackListDeleteArguments arguments)
		{
			var blacklist = (await Repository.GetAsync("TransportOperation"))
				.Where(x=> x.Source == BlackListSourceType.Payin && x.Resolved == false && x.Id == arguments.Id)
				.FirstOrDefault();

			blacklist.State = BlackListStateType.Delete;

			return blacklist;
		}
		#endregion ExecuteAsync
	}
}
