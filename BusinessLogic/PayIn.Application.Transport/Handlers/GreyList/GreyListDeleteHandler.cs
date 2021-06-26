using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class GreyListDeleteHandler : IServiceBaseHandler<GreyListDeleteArguments>
	{
		private readonly IEntityRepository<GreyList> Repository;

		#region Constructors
		public GreyListDeleteHandler(
			IEntityRepository<GreyList> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<GreyListDeleteArguments>.ExecuteAsync(GreyListDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = GreyList.GreyListStateType.Delete;

			return null;
		}
		#endregion ExecuteAsync
	}
}

