using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportCard;
using PayIn.Application.Dto.Transport.Results.TransportCard;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TransportCardGetByDeviceAddressHandler :
		IQueryBaseHandler<TransportCardGetByDeviceAddressArguments, TransportCardGetByDeviceAddressResult>
	{
		private readonly IEntityRepository<TransportCard> Repository;
		private readonly IEntityRepository<TransportPrice> RepositoryPrice;
		private readonly ISessionData SessionData;

		#region Constructor
		public TransportCardGetByDeviceAddressHandler(
			IEntityRepository<TransportCard> repository,
			IEntityRepository<TransportPrice> repositoryPrice,
			ISessionData sessionData
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPrice == null) throw new ArgumentNullException("repositoryPrice");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			RepositoryPrice = repositoryPrice;
			SessionData = sessionData;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<TransportCardGetByDeviceAddressResult>> ExecuteAsync(TransportCardGetByDeviceAddressArguments arguments)
		{
			var now = DateTime.UtcNow;

			var result = (await Repository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login && //Este login no es el correcto. Ha de ser el usuario de la tarjeta
					x.DeviceAddress == arguments.DeviceAddress &&
					x.State == TransportCardState.Active
				)
				.Select(x => new TransportCardGetByDeviceAddressResult
				{
					Id = x.Id,
					Name = x.Name,
					Entry = x.Entry,
					Uid = x.Uid,
					RandomId = x.RandomId
				});

			var rechargeTitles = (await RepositoryPrice.GetAsync())
				.Where(x =>
					x.Start <= now &&
					x.End >= now &&
					x.State == TransportPriceState.Active &&
					x.Title.OperateByPayIn &&
					x.Title.State == TransportTitleState.Active &&
					x.Title.TransportCardSupportTitleCompatibility
						.Select(y =>
							y.TransportCardSupport.OwnerCode == 0 &&
							y.TransportCardSupport.Type == 1 &&
							y.TransportCardSupport.SubType == 0
						)
						.Any() &&
					x.Version == (x.Title.Prices
						.Where(y =>
							y.Start <= now &&
							y.End >= now &&
							y.State == TransportPriceState.Active &&
							x.Zone == y.Zone
						)
						.Select(y => (int?)y.Version)
						.Max() ?? 0)
				)
				.GroupBy(x => x.Title)
					.Select(x => new
					{
						x.Key.Id,
						x.Key.Code,
						x.Key.Name,
						OwnerCity = "Valencia",
						x.Key.OwnerName,
						PaymentConcessionId = 1,
						TransportConcession = x.Key.TransportConcessionId,
						x.Key.HasZone,
						Prices = x
							.Select(y => new
							{
								y.Id,
								y.Zone,
								y.Price
							}),
						//TuiNMax = x.Key.MaxQuantity,
						//TuiNMin = x.Key.MinCharge,
						//TuiNStep = x.Key.PriceStep,
						x.Key.Quantity,
						x.Key.MeanTransport,
						x.Key.MaxQuantity,
						RechargeMin = x.Key.MinCharge,
						RechargeStep = x.Key.PriceStep,
						Slot = x.Key.Slot == 2 ?
							EigeTituloEnUsoEnum.Titulo2 :
							EigeTituloEnUsoEnum.Titulo1
					})
					.ToList()
					.Select(x => new ServiceCardReadInfoResult_RechargeTitle
					{
						Id = x.Id,
						Code = x.Code,
						Name = x.Name,
						OwnerCity = x.OwnerCity,
						OwnerName = x.OwnerName,
						PaymentConcessionId = x.PaymentConcessionId,
						TransportConcession = x.TransportConcession,
						Prices = x.Prices
							.Select(y => new ServiceCardReadInfoResult_RechargePrice
							{
								Id = y.Id,
								Zone = x.HasZone ? y.Zone : (x.Code == 96 ? EigeZonaEnum.A : (EigeZonaEnum?)null),
								ZoneName = x.HasZone ? y.Zone.ToEnumAlias("") : "",
								Price = y.Price,
								ChangePrice = 0,
								RechargeType = RechargeType.Charge,
								Slot = x.Slot
							}),
						//TuiNMax = x.TuiNMax,
						//TuiNMin = x.TuiNMin,
						//TuiNStep = x.TuiNStep,
						Quantity = x.Quantity,
						MeanTransport = x.MeanTransport,
						MaxQuantity = x.MaxQuantity,
						RechargeMin = x.RechargeMin,
						RechargeStep = x.RechargeStep
					});

			return new TransportCardGetByDeviceAddressResultBase
			{
				Data = result,
				RechargeTitles = rechargeTitles
			};
		}
		#endregion ExecuteAsync
	}
}
