using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class EntranceTypeCreateFormHandler : 
		IServiceBaseHandler<EntranceTypeFormCreateArguments>
	{
		private readonly IEntityRepository<EntranceTypeForm> Repository;

		#region Constructors
		public EntranceTypeCreateFormHandler(
			IEntityRepository<EntranceTypeForm> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeFormCreateArguments arguments)
		{
			var objs = (await Repository.GetAsync())
				.Where(x => 
					x.Order >= arguments.Order &&
					x.EntranceTypeId == arguments.EntranceTypeId
				)
				.ToList();

			if (objs.Where(x => x.Order == arguments.Order).Any())
					foreach (var obj in objs)
					obj.Order++;

			var item = new EntranceTypeForm
            {
                EntranceTypeId = arguments.EntranceTypeId,
                FormId = arguments.FormId,
                Order = arguments.Order
            };
            await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
