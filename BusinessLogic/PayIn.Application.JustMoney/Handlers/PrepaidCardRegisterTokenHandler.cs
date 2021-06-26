using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.BusinessLogic.Common;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.JustMoney;
using System.Linq;
using PayIn.Infrastructure.JustMoney.Enums;
using PayIn.Infrastructure.JustMoney.Services;
using System;

namespace PayIn.Application.JustMoney.Handlers
{
	public class PrepaidCardRegisterTokenHandler : IServiceBaseHandler<PrepaidCardRegisterTokenArguments>
	{
		//[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Option> OptionRepository { get; set; }
		//[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }
		[Dependency] public PfsService PfsService { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PrepaidCardRegisterTokenArguments arguments)
		{
			var option = (await OptionRepository.GetAsync(1));
			
			{
				var messageId = int.Parse(option.Value) + 1;
				option.Value = messageId.ToString();
				await UnitOfWork.SaveAsync();

				var registerToken = await PfsService.RegisterTokenAsync(
					messageId,
					Guid.NewGuid().ToString(), arguments.FirstName, arguments.LastName,
					arguments.Address1, arguments.Address2, arguments.ZipCode, arguments.City, arguments.State, arguments.Country,
					"http://www.google.es", "http://www.bing.es",
					""
				);
			}

			return new { };
		}
		#endregion ExecuteAsync
	}
}
