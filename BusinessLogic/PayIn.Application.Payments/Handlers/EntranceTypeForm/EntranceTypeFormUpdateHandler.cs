using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeFormUpdateHandler :
		IServiceBaseHandler<EntranceTypeFormUpdateArguments>
	{
		private readonly IEntityRepository<EntranceTypeForm> Repository;

		#region Constructors
		public EntranceTypeFormUpdateHandler(
			IEntityRepository<EntranceTypeForm> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeFormUpdateArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));
			var min = Math.Min(item.Order, arguments.Order);
			var max = Math.Max(item.Order, arguments.Order);

			var objs = (await Repository.GetAsync())
				.Where(x => 
					x.Order >= min &&
					x.Order <= max &&
					x.EntranceTypeId == arguments.EntranceTypeId
				)
				.ToList();

			if (objs.Where(x=> x.Order == arguments.Order).Any())
				foreach (var obj in objs)
				{
					if (obj.Order >= item.Order)
						obj.Order--;

					if (obj.Order >= arguments.Order)
						obj.Order++;
				}
			

			item.Order = arguments.Order;

			return item;
		}
		#endregion ExecuteAsync
	}
}
