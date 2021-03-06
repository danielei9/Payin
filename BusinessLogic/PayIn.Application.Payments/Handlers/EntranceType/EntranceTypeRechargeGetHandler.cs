using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeRechargeGetHandler :
		IQueryBaseHandler<EntranceTypeRechargeGetArguments, EntranceTypeRechargeGetResult>
	{
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
		[Dependency] public IEntityRepository<Purse> PurseRepository { get; set; }

		private string hexArray = "0123456789ABCDEF";

		#region ExecuteAsync
		public async Task<ResultBase<EntranceTypeRechargeGetResult>> ExecuteAsync(EntranceTypeRechargeGetArguments arguments)
		{
			var purses = (await PurseRepository.GetAsync())
				.Where(y =>
					y.SystemCard.Cards
						.Any(z => z.Id == arguments.CardId) &&
					y.IsRechargable &&
					!y.IsPayin
				)
				.Select(y => new SelectorResult
				{
					Id = y.Id,
					Value = y.Name
				})
				.ToList();

			var result = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.CardId)
				.Select(x => new 
				{
					Id = x.Id
				})
				.ToList()
				.Select(x => new EntranceTypeRechargeGetResult
				{
					Id = x.Id,
					PurseId = purses
						.Select(y => y.Id)
						.FirstOrDefault()
				});

			return new EntranceTypeRechargeGetResultBase {
				PurseId = purses,
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
