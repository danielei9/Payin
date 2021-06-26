using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments.Shop;
using PayIn.Application.Dto.Payments.Results.Shop;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handler.Shop
{
	public class ShopGetMyCardsSelectorHandler :
		IQueryBaseHandler<ShopGetMyCardsSelectorArguments, ShopGetMyCardsSelectorResult>
	{
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		
        #region ExecuteAsync
        public async Task<ResultBase<ShopGetMyCardsSelectorResult>> ExecuteAsync(ShopGetMyCardsSelectorArguments arguments)
        {
            if (arguments.Filter == "undefined")
                arguments.Filter = null;

			if (arguments.Filter.IsNullOrEmpty())
				return new ResultBase<ShopGetMyCardsSelectorResult> { Data = { } };

			var serviceCards = (await Repository.GetAsync())
				.Where(x =>
					x.Uid.ToString().Contains(arguments.Filter) ||
					x.Alias.Contains(arguments.Filter)
				);


			var result = serviceCards
					.OrderBy(x => new
					{
						x.Alias,
						x.Uid
					})
					.Select(x => new
					{
						x.Id,
						x.Uid,
						x.Alias
					})
					.ToList()
					.Select(x => new ShopGetMyCardsSelectorResult
					{
						Id = x.Id,
						Uid = x.Uid,
						Alias = x.Alias
					});

            return new ResultBase<ShopGetMyCardsSelectorResult> { Data = result };
        }
        #endregion ExecuteAsync
    }
}
