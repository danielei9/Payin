using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PublicCampaignGetByUserHandler :
		IQueryBaseHandler<PublicCampaignGetByUserArguments, PublicCampaignGetByUserResult>
	{
		[Dependency] public IEntityRepository<Campaign> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<PublicCampaignGetByUserResult>> ExecuteAsync(PublicCampaignGetByUserArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.Since <= arguments.Now) &&
					(x.Until >= arguments.Now) &&
					(x.State == CampaignState.Active) &&
					(
						// Sólo para Propietario o trabajador
						(x.Concession.Concession.Supplier.Login == SessionData.Login) ||
						(x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login))
					) &&
					// Comprobar cardSystem
					(
						(x.TargetSystemCardId == null) ||
						(
							(x.TargetSystemCard.SystemCardMembers.Any(y => y.Login == SessionData.Login)) &&
							(x.TargetSystemCard.Cards.Any(y => y.Users.Any(z => z.Login == arguments.Login)))
						)
					) && (
						(arguments.EventId == null) ||
						(x.TargetEvents.Count() == 0) ||
						(x.TargetEvents.Any(y => y.Id == arguments.EventId))
					)
				);
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x =>
					x.Title.Contains(arguments.Filter)
				);

			var result = items
			.SelectMany(x => x.CampaignLines
				.Select(y => new
				{
					Id = y.Id,
					Title = x.Title,
					Since = y.SinceTime,
					Until = y.UntilTime,
					Login = arguments.Login,
					Caducity = x.Until,
					Type = y.Type,
					Quantity = y.Quantity,
					AllProducts = y.AllProduct,
					Products =
						(
							y.Products.Select(a => a.Code)
						).Union(
							y.ProductFamilies.SelectMany(z => z.Products.Select(a => a.Code))
						).Union(
							y.ProductFamilies.SelectMany(z => z.SubFamilies.SelectMany(b => b.Products.Select(a => a.Code)))
						),
					AllEntrances = y.AllEntranceType,
					Entrances = y.EntranceTypes
						.Where(a => a.EventId == arguments.EventId)
						.Select(a => a.Id)
				})
			)
			.ToList();

		var result2 = result
			.Select(x => new
			{
				Id = x.Id,
				Title = x.Title,
				Since = x.Since,
				Until = x.Until,
				Caducity = x.Caducity,
				Qr = new PublicCampaignGetByUserResult_Qr
				{
					Id = x.Id //.ToBase32(),
					//Since = x.Since == null ? null : (Convert.ToInt32(x.Since?.ToUniversalTime().ToString("HHmm"))).ToBase32(),
					//Until = x.Until == null ? null : (Convert.ToInt32(x.Until?.ToUniversalTime().ToString("HHmm"))).ToBase32(),
					//Login = x.Login,
					//EventId = arguments.EventId.ToBase32(),
					//Caducity = x.Caducity == null ? null : (Convert.ToInt64(x.Caducity.ToUniversalTime().ToString("yyyyMMddHHmm"))).ToBase32(),
					//Type = ((int)x.Type).ToBase32(),
					//Quantity = ((int)(x.Quantity * 100)).ToBase32(),
					//Products = (
					//	x.AllProducts ?	"*" :
					//	x.Products.Count() == 0 ? null :
					//	x.Products.JoinString(",")
					//),
					//Entrances = (
					//	x.AllEntrances ? "*" :
					//	x.Entrances.Count() == 0 ? null :
					//	x.Entrances
					//		.Select(y => y.ToBase32())
					//		.JoinString(",")
					//)
				}
			})
#if DEBUG
			.ToList()
#endif //DEBUG
			;

			var resultBase = new ResultBase<PublicCampaignGetByUserResult>
			{
				Data = result2
					.Select(x => new PublicCampaignGetByUserResult
					{
						Id = x.Id,
						Title = x.Title,
						Since = x.Since,
						Until = x.Until,
						Caducity = x.Caducity,
						Qr = GenerateQr(x.Qr)
					})
			};
			return resultBase;
		}
#endregion ExecuteAsync

#region GenerateQr
		public static string GenerateQr(PublicCampaignGetByUserResult_Qr data)
		{
			var result = "pay[in]/promo:" + data.ToJson();
			return result;
		}
#endregion GenerateQr
	}
}
