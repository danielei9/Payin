using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	class ControlFormOptionDeleteHandler :
		IServiceBaseHandler<ControlFormOptionDeleteArguments>
	{
		private readonly IEntityRepository<ControlFormOption> Repository;

		#region Constructors
		public ControlFormOptionDeleteHandler(IEntityRepository<ControlFormOption> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlFormOptionDeleteArguments>.ExecuteAsync(ControlFormOptionDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = ControlFormOptionState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}
