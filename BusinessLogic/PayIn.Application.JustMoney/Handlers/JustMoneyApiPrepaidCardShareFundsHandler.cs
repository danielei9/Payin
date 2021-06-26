using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.JustMoney.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.JustMoney;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardShareFundsHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardShareFundsArguments>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public SessionData SessionData { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardShareFundsArguments arguments)
		{
			var user = await SecurityRepository.GetUserAsync(SessionData.Login, arguments.Password);
			if (user == null)
				throw new ApplicationException("Password is not correct");

			var source = (await PrepaidCardRepository.GetAsync(arguments.Id));
			if (source == null)
				throw new ArgumentNullException("Id");
			
			var target = (await PrepaidCardRepository.GetAsync())
				.Where(x => x.CardHolderId == arguments.TargetCardHolderId)
				.FirstOrDefault();
			if (target == null)
				throw new ArgumentNullException("targetCardHolderId");
			if (source.Id == target.Id)
				throw new ArgumentException("La tarjeta destino no puede ser la misma que la origen", "targetCardHolderId");

			var result = await PfsService.CardToCardAsync(source.CardHolderId, target.CardHolderId, arguments.Amount);
			
			return source;
        }
		#endregion ExecuteAsync
	}
}
