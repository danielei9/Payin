using PayIn.Application.Transport.Scripts;
using PayIn.Application.Transport.Services;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;
using static PayIn.Application.Transport.Handlers.TransportOperationReadInfoHandler;

namespace PayIn.Application.Transport.Handlers
{
	public abstract class TransportOperationInternal
	{
		protected readonly EigeService EigeService;
		protected readonly IEntityRepository<TransportPrice> PriceRepository;
        protected readonly IEntityRepository<BlackList> BlackListRepository;
        protected readonly IEntityRepository<GreyList> GreyListRepository;

		#region Constructors
		public TransportOperationInternal(
			EigeService eigeService,
			IEntityRepository<TransportPrice> priceRepository,
			IEntityRepository<BlackList> blackListRepository,
			IEntityRepository<GreyList> greyListRepository
		)
		{
			if (eigeService == null) throw new ArgumentNullException("eigeService");
			if (priceRepository == null) throw new ArgumentNullException("priceRepository");
			if (blackListRepository == null) throw new ArgumentNullException("blackListRepository");
			if (greyListRepository == null) throw new ArgumentNullException("greyListRepository");

			EigeService = eigeService;
			PriceRepository = priceRepository;
			BlackListRepository = blackListRepository;
			GreyListRepository = greyListRepository;
		}
		#endregion Constructors

		#region GetRechargesAsync
		protected async Task<TransportPrice> GetRechargesAsync(
			long uid,
			TransportCardGetReadInfoScript script,
			DateTime now,
			//RechargeType rechargeType,
			int? titleCode,
			IEnumerable<int> priceId,
			int rechargeNumber,
			bool checkMaxBalance = true
		)
		{
			var price = (await EigeService.GetRechargesAsync(
				uid,
				script,
				now,
				RechargeType.Charge,
				titleCode,
				priceId,
				rechargeNumber,
				checkMaxBalance
			))
			.FirstOrDefault();

			return price;
		}
		#endregion GetRechargesAsync

		#region GetRechargeConfig
		protected async Task<RechargeConfig> GetRechargeConfigAsync(
			long uid,
			TransportCardGetReadInfoScript script,
			DateTime now,
			TransportPrice price
		)
		{
			var titleCode1 = await EigeService.GetTitleCode1Async(uid, script);
			var titleZone1 = await EigeService.GetTitleZoneName1Async(uid, script);
			var titleTarifa1 = script.Card.Titulo.ControlTarifa1.Value;
			var titleActive1 = await EigeService.GetTitleActive1Async(uid, script);
			var titleIsExhausted1 = await EigeService.GetTitleIsExhausted1Async(uid, script, now);
			var titlePrice1 = (await PriceRepository.GetAsync("Title"))
				.Where(x =>
					x.Title.Code == titleCode1 &&
					(!x.Title.HasZone || x.Zone == titleZone1) &&
					x.Version == titleTarifa1
				)
				.FirstOrDefault();

			var titleCode2 = await EigeService.GetTitleCode2Async(uid, script);
			var titleZone2 = await EigeService.GetTitleZoneName2Async(uid, script);
			var titleTarifa2 = script.Card.Titulo.ControlTarifa2.Value;
			var titleActive2 = await EigeService.GetTitleActive2Async(uid, script);
			var titleIsExhausted2 = await EigeService.GetTitleIsExhausted2Async(uid, script, now);
			var titlePrice2 = (await PriceRepository.GetAsync("Title"))
				.Where(x =>
					x.Title.Code == titleCode2 &&
					(!x.Title.HasZone || x.Zone == titleZone2) &&
					x.Version == titleTarifa2
				)
				.FirstOrDefault();

			var config = EigeService.GetRechargeConfig(uid, script, titleActive1, titleIsExhausted1, titlePrice1, titleActive2, titleIsExhausted2, titlePrice2, price, now);

			return config;
		}
		#endregion GetRechargeConfig

		#region IsCompatibleSupport
		protected async Task<bool> IsCompatibleSupport(
			long uid,
			TransportCardGetReadInfoScript script,
			int titleCode)
		{
			var cardType = Convert.ToInt32(script.Card.Tarjeta.Tipo.Value);
			var cardSubtype = Convert.ToInt32(script.Card.Tarjeta.Subtipo.Value);
			var cardOwner = Convert.ToInt32(script.Card.Tarjeta.EmpresaPropietaria.Value);			
			
			if (cardSubtype != 0)
			{
				return (await PriceRepository.GetAsync())
					.Where(x =>
						x.Title.Code == titleCode &&
						x.Title.TransportCardSupportTitleCompatibility
						.Where(y =>
							y.TransportCardSupport.Type == cardType &&
							y.TransportCardSupport.SubType == cardSubtype &&
							y.TransportCardSupport.OwnerCode == cardOwner
						)
						.Any()
					)
					.Any();
			}
			else
			{
				return (await PriceRepository.GetAsync())
					.Where(x =>
						x.Title.Code == titleCode &&
						x.Title.TransportCardSupportTitleCompatibility
						.Where(y =>
							y.TransportCardSupport.Type == cardType &&
							y.TransportCardSupport.SubType == null &&
							y.TransportCardSupport.OwnerCode == cardOwner
						)
						.Any()
					)
					.Any();
			}
		}
		#endregion IsCompatibleSupport

		#region InBlackList
		protected async Task<bool> InBlackListAsync(
			long uid,
			TransportCardGetReadInfoScript script
		)
		{
			var inBlackList = (await BlackListRepository.GetAsync())
				.Where(x =>
					x.Uid == uid &&
					x.Machine == (x.Machine | BlackListMachineType.Charge) &&
					!x.Resolved
				)
				.Any();

			return (
				(inBlackList || script.Card.Validacion.ListaNegra.Value) &&
				(!script.Card.Usuario.DesbloqueoListaNegra.Value)
			);
		}
		#endregion InBlackList

		#region InGreyListAsync
		protected async Task<bool> InGreyListAsync(
			long uid,
			TransportCardGetReadInfoScript script
		)
		{
			var inGreyList = (await GreyListRepository.GetAsync())
				.Where(x => x.Uid == uid && !x.Resolved && x.State == GreyList.GreyListStateType.Active)
				.Any();


			// TODO: Revisar 
			return (inGreyList); // || script.Card.Validacion.ListaGris.Value);
		}
		#endregion InGreyListAsync
	}
}
