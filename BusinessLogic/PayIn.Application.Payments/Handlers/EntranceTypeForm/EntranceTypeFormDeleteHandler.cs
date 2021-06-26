using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class EntranceTypeFormDeleteHandler :
		IServiceBaseHandler<EntranceTypeFormDeleteArguments>
	{
		private readonly IEntityRepository<EntranceTypeForm> Repository;

		#region Constructors
		public EntranceTypeFormDeleteHandler(IEntityRepository<EntranceTypeForm> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeFormDeleteArguments arguments)
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
