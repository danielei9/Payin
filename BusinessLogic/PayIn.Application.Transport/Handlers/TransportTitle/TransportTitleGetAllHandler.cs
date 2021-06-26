using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
using PayIn.Application.Dto.Transport.Results.TransportTitle;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportTitleGetAllHandler : IQueryBaseHandler<TransportTitleGetAllArguments, TransportTitleGetAllResult>
	{

		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportTitle> Repository;
		private readonly IEntityRepository<TransportPrice> RepositoryTransportPrice;
		private readonly IEntityRepository<TransportCardSupportTitleCompatibility> RepositoryTransportCardSupportTitleCompatibility;
        private readonly IEntityRepository<TransportSimultaneousTitleCompatibility> RepositoryTransportSimultaneousTitleCompatibilities;


        #region Constructors
        public TransportTitleGetAllHandler(
			ISessionData sessionData,
			IEntityRepository<TransportTitle> repository,
			IEntityRepository<TransportPrice> repositoryTransportPrice,
            IEntityRepository<TransportSimultaneousTitleCompatibility> repositoryTransportSimultaneousTitleCompatibilities,
            IEntityRepository<TransportCardSupportTitleCompatibility> repositoryTransportCardSupportTitleCompatibility)

		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryTransportPrice == null) throw new ArgumentNullException("repositoryTransportPrice");
			if (repositoryTransportCardSupportTitleCompatibility == null) throw new ArgumentNullException("repositoryTransportCardSupportTitleCompatibility");
            if (repositoryTransportSimultaneousTitleCompatibilities == null) throw new ArgumentNullException("RepositoryTransportSimultaneousTitleCompatibilities");

            SessionData = sessionData;
			Repository = repository;
			RepositoryTransportPrice = repositoryTransportPrice;
			RepositoryTransportCardSupportTitleCompatibility = repositoryTransportCardSupportTitleCompatibility;
            RepositoryTransportSimultaneousTitleCompatibilities = repositoryTransportSimultaneousTitleCompatibilities;

        }
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<TransportTitleGetAllResult>> IQueryBaseHandler<TransportTitleGetAllArguments, TransportTitleGetAllResult>.ExecuteAsync(TransportTitleGetAllArguments arguments)
		{
			var titles = (await Repository.GetAsync())
				.Where(x => 
					x.TransportConcession.Concession.Concession.Supplier.Login.Contains(AccountRoles.Transport) || 
					SessionData.Login.Contains(AccountRoles.Superadministrator));
			var prices = (await RepositoryTransportPrice.GetAsync());
			var CardSupport = (await RepositoryTransportCardSupportTitleCompatibility.GetAsync());
            var TitleSimultaneous = (await RepositoryTransportSimultaneousTitleCompatibilities.GetAsync());

            if (!arguments.Filter.IsNullOrEmpty())
				titles = titles.Where(x => 
					x.Code.ToString() == arguments.Filter ||
					x.Name.Contains(arguments.Filter) ||
					x.OwnerName.Contains(arguments.Filter)
				);				

			var result = titles
				.OrderByDescending(x => x.State)
				.Select(x => new
				{
					Id = x.Id,
					Code = x.Code,
					Name = x.Name,
					OwnerCode = x.OwnerCode,
					OwnerName = x.OwnerName,
					Image = x.Image,
					HasZone = x.HasZone,
					State = x.State,
					CounterPricing = prices.Where(y => y.TransportTitleId == x.Id).Count(),
					CounterCardSupport = CardSupport.Where(y => y.TransportTitleId == x.Id).Count(),
                    CounterTitlecompatibilities = TitleSimultaneous.Where(y => y.TransportTitleId == x.Id).Count(),
                    MaxQuantity = x.MaxQuantity,
					OperateByPayIn = x.OperateByPayIn,
					IsYoung = x.IsYoung,
					Environment = x.Environment,
					IsOverWritable = x.IsOverWritable,
					ValidityBit = x.ValidityBit,
					TableIndex = x.TableIndex,
					MaxExternalChanges = x.MaxExternalChanges,
					MaxPeopleChanges = x.MaxPeopleChanges,
					ActiveTitle = x.ActiveTitle,
					Priority = x.Priority,
					TemporalTypeEnum = x.TemporalType,
					TemporalUnit = x.TemporalUnit,
					QuantityTypeEnum = x.QuantityType,
					Quantity = x.Quantity,

				})
				.ToList()
				.Select(x => new TransportTitleGetAllResult
				{
					Id = x.Id,
					Code = x.Code,
					Name = x.Name,
					OwnerCode = x.OwnerCode,
					OwnerName = x.OwnerName,
					Image = x.Image,
					HasZone = x.HasZone,
					State = x.State,
					CounterPricing = x.CounterPricing,
					MaxQuantity = x.MaxQuantity,
					OperateByPayIn = x.OperateByPayIn,
					IsYoung = x.IsYoung,
					Environment = x.Environment,
					IsOverWritable = x.IsOverWritable,
					EnvironmentAlias = (x.Environment == null ? "" : x.Environment.ToEnumAlias()),
					Price = x.CounterPricing == 1 ? (decimal?)prices.Where(y => y.TransportTitleId == x.Id).FirstOrDefault().Price : null,
					CounterCardSupport = x.CounterCardSupport,
                    CounterTitlecompatibilities = x.CounterTitlecompatibilities,
                    PriceId  = x.CounterPricing == 1 ? (int?)prices.Where(y => y.TransportTitleId == x.Id).FirstOrDefault().Id : null,
					ValidityBit = x.ValidityBit,
					TableIndex = x.TableIndex,
					MaxExternalChanges = x.MaxExternalChanges,
					MaxPeopleChanges = x.MaxPeopleChanges,
					ActiveTitle = x.ActiveTitle,
					Priority = x.Priority,
					TemporalType = x.TemporalTypeEnum,
					TemporalTypeAlias = ((x.TemporalTypeEnum == null) ? "" : x.TemporalTypeEnum.ToEnumAlias()),
					TemporalUnit = x.TemporalUnit,
					QuantityType = x.QuantityTypeEnum,
					QuantityTypeAlias = ((x.QuantityTypeEnum == null) ? "" : x.QuantityTypeEnum.ToEnumAlias()),
					Quantity = x.Quantity,
				})
				.OrderBy(x => x.OwnerName).ThenBy(x => x.Name);

			return new ResultBase<TransportTitleGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

