using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.JustMoney.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class MainGetAllHandler : IQueryBaseHandler<MainGetAllArguments, MainGetAllResult>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public IEntityRepository<Option> OptionRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MainGetAllResult>> ExecuteAsync(MainGetAllArguments arguments)
		{
			var option = (await OptionRepository.GetAsync(1));

			var list = (await PrepaidCardRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login
				)
				.Select(x => new
				{
					x.Id,
					x.InternalToken
				})
				.Take(1)
				.ToList();

			var result = new List<MainGetAllResult>();
			foreach(var item in list)
			{
				var messageId = int.Parse(option.Value) + 1;
				option.Value = messageId.ToString();
				await UnitOfWork.SaveAsync();

				var resultBalance = await PfsService.CardInquiryAsync(messageId, item.InternalToken);
				result.Add(new MainGetAllResult
				{
					Id = item.Id,
					Balance = resultBalance.CardInfo.AvailBal,
					Status = resultBalance.CardInfo.CardStatus
				});
			}

			return new ResultBase<MainGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
