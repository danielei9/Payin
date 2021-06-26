using Microsoft.Practices.Unity;
using PayIn.Application.Dto.JustMoney.Arguments;
using PayIn.Application.Dto.JustMoney.Results;
using PayIn.Domain.JustMoney;
using PayIn.Domain.JustMoney.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.JustMoney.Handlers
{
	public class JustMoneyApiPrepaidCardUpdatelHandler : IServiceBaseHandler<JustMoneyApiPrepaidCardUpdateArguments>
	{
		[Dependency] public IEntityRepository<PrepaidCard> PrepaidCardRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(JustMoneyApiPrepaidCardUpdateArguments arguments)
        {
            var item = (await PrepaidCardRepository.GetAsync(arguments.Id));
            if (item == null)
                throw new ArgumentNullException("Id");

            item.Login = arguments.Login;

            return item;
        }
        #endregion ExecuteAsync
    }
}
